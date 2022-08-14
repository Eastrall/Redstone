namespace Redstone.Common.Chat;

/// <summary>
/// Represents the available chat message types.
/// </summary>
public enum ChatMessageType
{
    /// <summary>
    /// A player-initiated chat message.
    /// </summary>
    Chat,

    /// <summary>
    /// System chat message in response of a chat command.
    /// </summary>
    System,

    /// <summary>
    /// Game state information that is displayed above the hot bar, such as "You may not rest now, the bed is too far away".
    /// </summary>
    GameInfo
}
