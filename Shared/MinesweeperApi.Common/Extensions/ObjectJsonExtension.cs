namespace MinesweeperApi.Common.Extensions
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;

    /// <summary>
    /// Provides extension methods for JSON serialization and deserialization using Newtonsoft.Json.
    /// </summary>
    public static class ObjectJsonExtension
    {
        /// <summary>
        /// Configures the provided <see cref="JsonSerializerSettings"/> instance with default settings.
        /// This includes using camel case property naming and ignoring reference loops.
        /// </summary>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> instance to configure.</param>
        /// <returns>The configured <see cref="JsonSerializerSettings"/> instance.</returns>
        public static JsonSerializerSettings SetDefaultSettings(this JsonSerializerSettings settings)
        {
            // Use camel case naming convention for JSON properties.
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Ignore reference loops during serialization.
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }

        /// <summary>
        /// Creates a new <see cref="JsonSerializerSettings"/> instance with the default configuration.
        /// </summary>
        /// <returns>A new <see cref="JsonSerializerSettings"/> instance configured with default settings.</returns>
        public static JsonSerializerSettings DefaultJsonSerializerSettings()
        {
            return new JsonSerializerSettings().SetDefaultSettings();
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using the provided settings or the default settings if none are provided.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="settings">Optional <see cref="JsonSerializerSettings"/> to use during serialization.</param>
        /// <returns>A JSON string representation of the object.</returns>
        /// <exception cref="JsonException">Thrown when serialization fails.</exception>
        public static string ToJsonString(this object obj, JsonSerializerSettings settings = null)
        {
            try
            {
                // Use the provided settings or the default settings if null.
                return JsonConvert.SerializeObject(obj, settings ?? DefaultJsonSerializerSettings());
            }
            catch (Exception ex)
            {
                throw new JsonException("Failed to convert to JSON string", ex);
            }
        }

        /// <summary>
        /// Deserializes the JSON string into an object of type <typeparamref name="T"/> using the provided settings or the default settings if none are provided.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="obj">The JSON string to deserialize.</param>
        /// <param name="settings">Optional <see cref="JsonSerializerSettings"/> to use during deserialization.</param>
        /// <returns>An object of type <typeparamref name="T"/> deserialized from the JSON string.</returns>
        public static T FromJsonString<T>(this string obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(obj, settings ?? DefaultJsonSerializerSettings());
        }

        /// <summary>
        /// Deserializes the JSON string into an object using the provided settings or the default settings if none are provided.
        /// </summary>
        /// <param name="obj">The JSON string to deserialize.</param>
        /// <param name="settings">Optional <see cref="JsonSerializerSettings"/> to use during deserialization.</param>
        /// <returns>An object deserialized from the JSON string.</returns>
        public static object FromJsonString(this string obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject(obj, typeof(object), settings ?? DefaultJsonSerializerSettings());
        }

        /// <summary>
        /// Attempts to deserialize the JSON string into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON string into.</typeparam>
        /// <param name="obj">The JSON string to deserialize.</param>
        /// <param name="result">
        /// When this method returns, contains the deserialized object of type <typeparamref name="T"/> if the deserialization succeeded; otherwise, the default value for <typeparamref name="T"/>.
        /// </param>
        /// <returns><c>true</c> if deserialization was successful; otherwise, <c>false</c>.</returns>
        public static bool TryFromJsonString<T>(this string obj, out T? result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(obj);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
