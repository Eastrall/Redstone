using System;

namespace Redstone.Common.Server;

public class MinecraftServerPlayer
{
    /// <summary>
    /// Gets or sets the player id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the player name.
    /// </summary>
    public string Name { get; set; }
}
