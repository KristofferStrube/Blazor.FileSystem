using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem
{
    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dictdef-filesystemremoveoptions">FileSystemRemoveOptions browser specs</see>
    /// </summary>
    public class FileSystemRemoveOptions
    {
        [JsonPropertyName("recursive")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Recursive { get; set; }
    }
}
