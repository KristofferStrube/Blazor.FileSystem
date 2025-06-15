using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem
{
    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dictdef-filesystemcreatewritableoptions">FileSystemCreateWritableOptions browser specs</see>
    /// </summary>
    public class FileSystemCreateWritableOptions
    {
        [JsonPropertyName("keepExistingData")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool KeepExistingData { get; set; }
    }
}
