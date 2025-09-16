using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The options for getting a <see cref="FileSystemDirectoryHandle"/> using the <see cref="FileSystemDirectoryHandle.GetDirectoryHandleAsync(string, KristofferStrube.Blazor.FileSystem.FileSystemGetDirectoryOptions?)"/> method.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetdirectoryoptions">See the API definition here</see>.</remarks>
public class FileSystemGetDirectoryOptions
{
    /// <summary>
    /// If the directory does not already exist and this is set to <see langword="true"/> it will create the directory; Else it will fail.
    /// </summary>
    [JsonPropertyName("create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Create { get; set; }
}
