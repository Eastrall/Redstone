using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using System;

namespace Redstone.Server.Handlers.Play;

public class HeldItemChangeHandler
{
    private readonly ILogger<HeldItemChangeHandler> _logger;

    public HeldItemChangeHandler(ILogger<HeldItemChangeHandler> logger)
    {
        _logger = logger;
    }

    [PlayPacketHandler(ServerPlayPacketType.HeldItemChange)]
    public void OnHeldItemChange(IMinecraftUser user, IMinecraftPacket packet)
    {
        short slot = packet.ReadInt16();

        if (slot < 0 || slot > 8)
        {
            throw new IndexOutOfRangeException($"Slot was out of bounds: {slot}");
        }

        _logger.LogDebug($"Current slot: {slot}");

        user.Player.HotBar.SetSlotIndex(slot);

        _logger.LogDebug($"Selected Item: ItemId = {user.Player.HotBar.SelectedSlot.ItemId}");
    }
}
