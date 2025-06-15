using KristofferStrube.Blazor.FileAPI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem.Converters
{
    internal class BlobConverter : JsonConverter<Blob>
    {
        public override Blob Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Does not support deserializing Blobs");
        }

        public override void Write(Utf8JsonWriter writer, Blob value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.JSReference, options);
        }
    }
}
