using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem.Converters;

internal class WriteCommandTypeConverter : JsonConverter<WriteCommandType>
{
    public override WriteCommandType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "write" => WriteCommandType.Write,
            "seek" => WriteCommandType.Seek,
            "truncate" => WriteCommandType.Truncate,
            var value => throw new ArgumentException($"Value '{value}' was not a valid {nameof(WriteCommandType)}."),
        };
    }

    public override void Write(Utf8JsonWriter writer, WriteCommandType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            WriteCommandType.Write => "write",
            WriteCommandType.Seek => "seek",
            WriteCommandType.Truncate => "truncate",
            _ => throw new ArgumentException($"Value '{value}' was not a valid {nameof(WriteCommandType)}.")
        });
    }
}
