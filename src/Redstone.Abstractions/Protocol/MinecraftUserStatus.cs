namespace Redstone.Abstractions.Protocol;

/// <summary>
/// Describes the different states of a Minecraft user.
/// </summary>
public enum MinecraftUserStatus : int
{
    Handshaking,
    Status,
    Login,
    Play
}
