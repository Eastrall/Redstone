using System.Text.Json;
using System.Text.Json.Serialization;

namespace Redstone.Common.Serialization
{
    /// <summary>
    /// Provides methods to serialize and deserialize json related content.
    /// </summary>
    public static class JsonSerializer
    {
        private static readonly JsonSerializerOptions _jsonDefaultOptions;

        /// <summary>
        /// Initializes the default JSON configuration options.
        /// </summary>
        static JsonSerializer()
        {
            _jsonDefaultOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            _jsonDefaultOptions.Converters.Add(new JsonStringEnumConverter());
        }

        /// <summary>
        /// Deserializes the given json content into a <typeparamref name="TObject"/> object.
        /// </summary>
        /// <typeparam name="TObject">Destination object type.</typeparam>
        /// <param name="jsonContent">JSON content to deserialize.</param>
        /// <param name="options">JSON serializer options. Note that this options overrides the default options.</param>
        /// <returns>Deserialized object.</returns>
        public static TObject Deserialize<TObject>(string jsonContent, JsonSerializerOptions options = null)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TObject>(jsonContent, options ?? _jsonDefaultOptions);
        }

        /// <summary>
        /// Serializes the given object into a valid JSON string.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <param name="object">Object to serialize.</param>
        /// <param name="options">JSON serializer options. Note that this options overrides the default options.</param>
        /// <returns>Object serialized as JSON.</returns>
        public static string Serialize<TObject>(TObject @object, JsonSerializerOptions options = null)
        {
            return System.Text.Json.JsonSerializer.Serialize(@object, options ?? _jsonDefaultOptions);
        }
    }
}
