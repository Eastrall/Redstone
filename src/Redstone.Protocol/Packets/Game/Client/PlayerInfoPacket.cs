using Redstone.Abstractions.Entities;
using Redstone.Common;
using Redstone.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class PlayerInfoPacket : MinecraftPacket
    {
        public PlayerInfoPacket(PlayerInfoActionType actionType, IPlayer player)
            : this(actionType, new[] { player })
        {
        }

        public PlayerInfoPacket(PlayerInfoActionType actionType, IEnumerable<IPlayer> players)
            : base(ClientPlayPacketType.PlayerInfo)
        {
            WriteVarInt32((int)actionType);
            WriteVarInt32(players.Count());

            foreach (IPlayer player in players)
            {
                WriteUUID(player.Id);

                if (actionType is PlayerInfoActionType.Add)
                {
                    WriteString(player.Name.TakeCharacters(16));
                    WriteVarInt32(0); // Optional properties
                    WriteVarInt32((int)player.GameMode);
                    WriteVarInt32(player.Ping);
                    WriteBoolean(false); // Has display name

                    // TODO: if HasDisplayName is true
                    // WriteString(player.DisplayName);
                }
                else if (actionType is PlayerInfoActionType.UpdateLatency)
                {
                    WriteVarInt32(player.Ping);
                }
            }
        }
    }
}
