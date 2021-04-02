using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Server.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redstone.Server.World
{
    public class WorldMap : IWorldMap
    {
        public const int UpdateTickRate = 50;

        private readonly ConcurrentDictionary<Guid, IPlayer> _players;
        private readonly List<IRegion> _regions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorldMap> _logger;
        private readonly float _entityVisibilityRange = 45f;

        private bool _isUpdating;
        private Task _updateTask;
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;

        public IEnumerable<IRegion> Regions => _regions;

        public IEnumerable<IPlayer> Players => _players.Values.AsEnumerable();

        public bool IsUpdating => _isUpdating;

        public string Name { get; }

        public WorldMap(string worldName, IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(worldName))
            {
                throw new ArgumentException($"'{nameof(worldName)}' cannot be null or empty", nameof(worldName));
            }

            Name = worldName;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = _serviceProvider.GetRequiredService<ILogger<WorldMap>>();
            _regions = new List<IRegion>();
            _players = new ConcurrentDictionary<Guid, IPlayer>();
        }

        public IRegion AddRegion(int x, int z)
        {
            if (ContainsRegion(x, z))
            {
                throw new InvalidOperationException($"Region {x}/{z} already exists.");
            }

            var region = new Region(x, z, _serviceProvider);

            _regions.Add(region);

            return region;
        }

        public IRegion GetRegion(int x, int z) => _regions.FirstOrDefault(region => region.X == x && region.Z == z);

        public bool ContainsRegion(int x, int z) => _regions.Any(region => region.X == x && region.Z == z);

        public void AddPlayer(IPlayer player)
        {
            if (player is Player playerEntity)
            {
                playerEntity.Map = this;
            }

            if (!_players.TryAdd(player.Id, player))
            {
                throw new InvalidOperationException($"Cannot add player with id: {player.Id} to the current map. Player already exists.");
            }
        }

        public IPlayer RemovePlayer(IPlayer player)
            => _players.TryRemove(player.Id, out IPlayer removedPlayer) ?
                removedPlayer :
                throw new InvalidOperationException($"Failed to remove player with id: {player.Id} from map {Name}. Player doesn't exist.");

        public IPlayer GetPlayer(Guid playerId)
            => _players.TryGetValue(playerId, out IPlayer player) ? player : default;

        public IEnumerable<IEntity> GetVisibleEntities(IEntity entity)
        {
            var entities = new List<IEntity>();

            lock (_players)
            {
                entities.AddRange(GetVisibleEntities(entity, _players.Values, _entityVisibilityRange));
            }

            return entities;
        }

        public void StartUpdate()
        {
            if (_isUpdating)
            {
                throw new InvalidOperationException("Cannot start update because the current map is already updating.");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _updateTask = Task.Factory.StartNew(
                () => Update(),
                _cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            _isUpdating = true;
        }

        public void StopUpdate()
        {
            if (!_isUpdating)
            {
                throw new InvalidOperationException("Cannot stop update because the current map is not being updated.");
            }

            _cancellationTokenSource.Cancel();
            _isUpdating = false;
        }

        public void Dispose()
        {
            if (_isUpdating)
            {
                StopUpdate();
            }

            _players.Clear();
            _regions.Clear();
        }

        public void Broadcast(IMinecraftPacket packet)
        {
            foreach (IPlayer player in _players.Values)
            {
                player.SendPacket(packet);
            }
        }

        private async Task Update()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(UpdateTickRate, _cancellationToken);

                    foreach (var playerEntity in _players)
                    {
                        IPlayer currentPlayer = playerEntity.Value;

                        currentPlayer.KeepAlive();
                        currentPlayer.LookAround();
                    }

                    // TODO: update monsters and animals
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"An error occured while updating map '{Name}'");
                }
            }

            _updateTask.Dispose();
        }

        private static IEnumerable<TEntity> GetVisibleEntities<TEntity>(IEntity currentEntity, IEnumerable<TEntity> entities, float visibilityRange)
            where TEntity : IEntity
        {
            return from x in entities
                   where x.EntityId != currentEntity.EntityId &&
                         x.IsSpawned &&
                         x.IsVisible &&
                         x.Position.IsInRange(currentEntity.Position, visibilityRange)
                   select x;
        }
    }
}
