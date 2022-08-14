namespace Redstone.Common.Server;

public class MinecraftServerVersion
{
    /// <summary>
    /// Gets the server version name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Minecraft protocol version number.
    /// </summary>
    public ushort Protocol { get; set; }
}
