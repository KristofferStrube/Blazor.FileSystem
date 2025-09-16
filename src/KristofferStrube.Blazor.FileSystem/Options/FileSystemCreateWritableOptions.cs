using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The options for creating a <see cref="FileSystemWritableFileStream"/> using the <see cref="FileSystemFileHandle.CreateWritableAsync(KristofferStrube.Blazor.FileSystem.FileSystemCreateWritableOptions?)"/> method.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemcreatewritableoptions">See the API definition here</see>.</remarks>
public class FileSystemCreateWritableOptions
{
    /// <summary>
    /// If set to <see langword="true"/> then it keeps the existing data of the <see cref="FileSystemFileHandle"/>.
    /// </summary>
    [JsonPropertyName("keepExistingData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool KeepExistingData { get; set; } = false;
}
