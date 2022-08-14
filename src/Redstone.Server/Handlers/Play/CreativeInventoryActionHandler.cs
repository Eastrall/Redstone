using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Protocol;
using Redstone.NBT.Tags;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play;

public class CreativeInventoryActionHandler
{
    private readonly ILogger<CreativeInventoryActionHandler> _logger;

    public CreativeInventoryActionHandler(ILogger<CreativeInventoryActionHandler> logger)
    {
        _logger = logger;
    }

    [PlayPacketHandler(ServerPlayPacketType.CreativeInventoryAction)]
    public void OnCreativeInventoryAction(IMinecraftUser user, IMinecraftPacket packet)
    {
        short slot = (short)(packet.ReadInt16() - RedstoneContants.PlayerInventoryHotbarOffset);
        // item slot structure
        bool present = packet.ReadBoolean();

        if (present)
        {
            int itemId = packet.ReadVarInt32();
            byte itemCount = packet.ReadByte();
            NbtCompound itemExtras = packet.ReadNbtCompound();

            _logger.LogInformation($"Item with id: {itemId} (x{itemCount}) set at slot {slot}");
            user.Player.HotBar.SetItem(slot, itemId, itemCount);
        }
        else
        {
            _logger.LogInformation($"Clearing item slot '{slot}'");
            user.Player.HotBar.ClearItem(slot);
        }
    }
}
