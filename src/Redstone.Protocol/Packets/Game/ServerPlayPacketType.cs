﻿namespace Redstone.Protocol.Packets.Game;

public enum ServerPlayPacketType
{
    TeleportConfirm = 0x00,
    QueryBlockNBT = 0x01,
    QueryEntityNBT = 0x0D,
    SetDifficulty = 0x02,
    ChatMessage = 0x03,
    ClientStatus = 0x04,
    ClientSettings = 0x05,
    TabComplete = 0x06,
    WindowConfirmation = 0x07,
    ClickWindowButton = 0x08,
    ClickWindow = 0x09,
    CloseWindow = 0x0A,
    PluginMessage = 0x0B,
    EditBook = 0x0C,
    InteractEntity = 0x0E,
    GenerateStructure = 0x0F,
    KeepAlive = 0x10,
    LockDifficulty = 0x11,
    PlayerPosition = 0x12,
    PlayerPositionAndRotation = 0x13,
    PlayerRotation = 0x14,
    PlayerMovement = 0x15,
    VehicleMove = 0x16,
    SteerBoat = 0x17,
    PickItem = 0x18,
    CraftRecipeRequest = 0x19,
    PlayerAbilities = 0x1A,
    PlayerDigging = 0x1B,
    EntityAction = 0x1C,
    SteerVehicle = 0x1D,
    SetDisplayedRecipe = 0x1E,
    SetRecipeBookState = 0x1F,
    NameItem = 0x20,
    ResourcePackStatus = 0x21,
    AdvancementTab = 0x22,
    SelectTrade = 0x23,
    SetBeaconEffect = 0x24,
    HeldItemChange = 0x25,
    UpdateCommandBlock = 0x26,
    UpdateCommandBlockMinecart = 0x27,
    CreativeInventoryAction = 0x28,
    UpdateJigsawBlock = 0x29,
    UpdateBlockStructure = 0x2A,
    UpdateSign = 0x2B,
    Animation = 0x2C,
    Spectate = 0x2D,
    PlayerBlockPlacement = 0x2E,
    UseItem = 0x2F
}
