using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The options for getting a <see cref="FileSystemFileHandle"/> using the <see cref="FileSystemDirectoryHandle.GetFileHandleAsync(string, KristofferStrube.Blazor.FileSystem.FileSystemGetFileOptions?)"/> method.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetfileoptions">See the API definition here</see>.</remarks>
public class FileSystemGetFileOptions
{
    /// <summary>
    /// If the file does not already exist and this is set to <see langword="true"/> it will create the file; Else it will fail.
    /// </summary>
    [JsonPropertyName("create")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Create { get; set; }
}
