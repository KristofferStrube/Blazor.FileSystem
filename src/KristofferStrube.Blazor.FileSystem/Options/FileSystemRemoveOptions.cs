using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The options for removing an entry in a <see cref="FileSystemDirectoryHandle"/> with the <see cref="FileSystemDirectoryHandle.RemoveEntryAsync(string, KristofferStrube.Blazor.FileSystem.FileSystemRemoveOptions?)"/> method.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemremoveoptions">See the API definition here</see>.</remarks>
public class FileSystemRemoveOptions
{
    /// <summary>
    /// Whether it should recursively remove subfolder in case the 
    /// </summary>
    [JsonPropertyName("recursive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Recursive { get; set; }
}
