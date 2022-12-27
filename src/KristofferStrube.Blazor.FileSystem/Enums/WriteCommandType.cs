using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href="https://fs.spec.whatwg.org/#enumdef-writecommandtype">WriteCommandType browser specs</see>
/// </summary>
[JsonConverter(typeof(EnumDescriptionConverter<WriteCommandType>))]
public enum WriteCommandType
{
    [Description("write")]
    Write,
    [Description("seek")]
    Seek,
    [Description("truncate")]
    Truncate,
}
