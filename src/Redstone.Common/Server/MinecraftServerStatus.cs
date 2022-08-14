namespace Redstone.Common.Server;

public class MinecraftServerStatus
{
    /// <summary>
    /// Gets or sets the Minecraft server version.
    /// </summary>
    public MinecraftServerVersion Version { get; set; }

    /// <summary>
    /// Gets or sets the Minecraft server players information.
    /// </summary>
    public MinecraftServerPlayers Players { get; set; }

    /// <summary>
    /// Gets or sets the Minecraft server status description.
    /// </summary>
    public MinecraftServerDescription Description { get; set; }

    /// <summary>
    /// Gets or sets the Minecraft server favicon.
    /// </summary>
    public string Favicon { get; set; }
}
