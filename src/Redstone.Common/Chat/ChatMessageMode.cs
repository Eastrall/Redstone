namespace Redstone.Common.Chat
{
    /// <summary>
    /// Represents the available chat message modes for client settings.
    /// </summary>
    public enum ChatMessageMode
    {
        /// <summary>
        /// The client is willing to accept all chat messages.
        /// </summary>
        Full,

        /// <summary>
        /// The client is willing to accept messages from commands, but does not want general chat from other players.
        /// </summary>
        System,

        /// <summary>
        /// The client does not want any chat at all. (However, it is still fine with above-hotbar game notices)
        /// </summary>
        None
    }
}
