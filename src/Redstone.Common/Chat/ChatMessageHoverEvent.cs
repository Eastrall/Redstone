using System.Text.Json.Serialization;

namespace Redstone.Common.Chat
{
    /// <summary>
    /// Represents an object that defines a chat message hover event.
    /// </summary>
    public class ChatMessageHoverEvent
    {
        /// <summary>
        /// Gets or sets the text to display.
        /// </summary>
        [JsonPropertyName("show_text")]
        public string ShowText { get; set; }

        /// <summary>
        /// Gets or sets the item as NBT string to display.
        /// </summary>
        [JsonPropertyName("show_item")]
        public string ShowItem { get; set; }

        /// <summary>
        /// Gets or sets the entity as JSON NBT string to display.
        /// </summary>
        [JsonPropertyName("show_entity")]
        public string ShowEntity { get; set; }
    }
}
