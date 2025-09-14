using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem.Converters;

internal class FileSystemHandleKindConverter : JsonConverter<FileSystemHandleKind>
{
    public override FileSystemHandleKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "file" => FileSystemHandleKind.File,
            "directory" => FileSystemHandleKind.Directory,
            var value => throw new ArgumentException($"Value '{value}' was not a valid {nameof(FileSystemHandleKind)}."),
        };
    }

    public override void Write(Utf8JsonWriter writer, FileSystemHandleKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            FileSystemHandleKind.File => "file",
            FileSystemHandleKind.Directory => "directory",
            _ => throw new ArgumentException($"Value '{value}' was not a valid {nameof(FileSystemHandleKind)}.")
        });
    }
}
