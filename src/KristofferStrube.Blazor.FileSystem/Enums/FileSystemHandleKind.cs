using KristofferStrube.Blazor.FileSystem.Converters;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The kind of a <see cref="FileSystemHandle"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#enumdef-filesystemhandlekind">See the API definition here</see>.</remarks>
[JsonConverter(typeof(FileSystemHandleKindConverter))]
public enum FileSystemHandleKind
{
    /// <summary>
    /// Used to describe that the entry is a file.
    /// </summary>
    File,

    /// <summary>
    /// Used to describe that the entry is a directory.
    /// </summary>
    Directory,
}