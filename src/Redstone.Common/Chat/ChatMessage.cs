using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Redstone.Common.Chat
{
    /// <summary>
    /// Represents a chat message object.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Gets or sets the chat text message.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the text should be bold.
        /// </summary>
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the text should be italic.
        /// </summary>
        [JsonPropertyName("italic")]
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the text should be undelined.
        /// </summary>
        [JsonPropertyName("underline")]
        public bool Underline { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the text should be stricked through.
        /// </summary>
        [JsonPropertyName("strikethrough")]
        public bool StrikeThrough { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the text should be obfuscated.
        /// </summary>
        [JsonPropertyName("obfuscated")]
        public string Obfuscated { get; set; }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets some additionnal text to insert into the current message.
        /// </summary>
        [JsonPropertyName("insertion")]
        public string Insertion { get; set; }

        /// <summary>
        /// Gets or sets a chat message click event.
        /// </summary>
        [JsonPropertyName("clickEvent")]
        public ChatMessageClickEvent ClickEvent { get; set; }

        /// <summary>
        /// Gets or sets a chat message hover event.
        /// </summary>
        [JsonPropertyName("hoverEvent")]
        public ChatMessageHoverEvent HoverEvent { get; set; }

        /// <summary>
        /// Gets or sets a collection of extra text.
        /// </summary>
        [JsonPropertyName("extras")]
        public IList<ChatMessage> Extras { get; set; }
    }
}
