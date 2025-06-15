using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem
{
    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetdirectoryoptions">FileSystemGetDirectoryOptions browser specs</see>
    /// </summary>
    public class FileSystemGetDirectoryOptions
    {
        [JsonPropertyName("create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Create { get; set; }
    }
}
