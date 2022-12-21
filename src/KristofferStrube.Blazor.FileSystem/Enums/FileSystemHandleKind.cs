using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href="https://fs.spec.whatwg.org/#enumdef-filesystemhandlekind">FileSystemHandleKind browser specs</see>
/// </summary>
[JsonConverter(typeof(EnumDescriptionConverter<FileSystemHandleKind>))]
public enum FileSystemHandleKind
{
    [Description("file")]
    File,
    [Description("directory")]
    Directory,
}