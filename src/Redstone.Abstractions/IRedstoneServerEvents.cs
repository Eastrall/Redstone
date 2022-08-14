using Redstone.Abstractions.Events;

namespace Redstone.Abstractions;

/// <summary>
/// Provides a mechanism that describes the different Minecraft events that can occur on the server.
/// </summary>
public interface IRedstoneServerEvents
{
    /// <summary>
    /// Triggers the event when a player joins the game.
    /// </summary>
    /// <param name="playerJoinEvent">Player join event arguments.</param>
    void OnPlayerJoinGame(PlayerJoinEventArgs playerJoinEvent);

    /// <summary>
    /// Triggers the event when a player leave the game.
    /// </summary>
    /// <param name="playerLeaveEvent">Player leave event arguments.</param>
    void OnPlayerLeaveGame(PlayerLeaveEventArgs playerLeaveEvent);
}
