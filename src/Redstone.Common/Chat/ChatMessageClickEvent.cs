using System.Text.Json.Serialization;

namespace Redstone.Common.Chat;

/// <summary>
/// Represents an object that defines a chat message click event.
/// </summary>
public class ChatMessageClickEvent
{
    /// <summary>
    /// Opens the given URL in the default web browser. 
    /// Ignored if the player has opted to disable links in chat; may open a GUI prompting the user if the setting for that is enabled. 
    /// </summary>
    /// <remarks>
    /// The link's protocol must be set and must be http or https, for security reasons.
    /// </remarks>
    [JsonPropertyName("open_url")]
    public string OpenUrl { get; set; }

    /// <summary>
    /// Runs the given command.
    /// </summary>
    /// <remarks>
    /// Not required to be a command - clicking this only causes the client to send the given content as a chat message, so if not prefixed with /, they will say the given text instead. 
    /// If used in a book GUI, the GUI is closed after clicking.
    /// </remarks>
    [JsonPropertyName("run_command")]
    public string RunCommand { get; set; }

    /// <summary>
    /// Suggest the command.
    /// </summary>
    /// <remarks>
    /// Only usable for messages in chat. 
    /// Replaces the content of the chat box with the given text - usually a command, but it is not required to be a command (commands should be prefixed with /).
    /// </remarks>
    [JsonPropertyName("suggest_command")]
    public string SuggestCommand { get; set; }

    /// <summary>
    /// Changes the current book page.
    /// </summary>
    [JsonPropertyName("change_page")]
    public int ChangePage { get; set; }

    /// <summary>
    /// Copies the given text to the client's clipboard when clicked.
    /// </summary>
    [JsonPropertyName("copy_to_clipboard")]
    public bool CopyToClipboard { get; set; }
}
