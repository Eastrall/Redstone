namespace Redstone.Common.Configuration;

/// <summary>
/// Provides a data structure for the chat option.
/// </summary>
public class ChatOptions
{
    /// <summary>
    /// Gets or sets the chat format.
    /// </summary>
    /// <remarks>
    /// Default is "<{0}> {1}" where 0 is the player name and 1 is the message.
    /// </remarks>
    public string Format { get; set; } = "<{0}> {1}";
}
