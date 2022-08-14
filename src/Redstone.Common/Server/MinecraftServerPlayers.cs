using System;
using System.Collections.Generic;

namespace Redstone.Common.Server;

public class MinecraftServerPlayers
{
    /// <summary>
    /// Gets or sets the maximum amount of players connected at the same time on this server.
    /// </summary>
    public uint Max { get; set; }

    /// <summary>
    /// Gets or sets the amount of players connected.
    /// </summary>
    public uint Online { get; set; }

    /// <summary>
    /// Gets or sets the connected player information.
    /// </summary>
    public IEnumerable<MinecraftServerPlayer> Sample { get; set; } = Array.Empty<MinecraftServerPlayer>();
}
