using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetfileoptions">FileSystemGetFileOptions browser specs</see>
/// </summary>
public class FileSystemGetFileOptions
{
    [JsonPropertyName("create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Create { get; set; }
}
