using KristofferStrube.Blazor.FileSystem.Converters;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// The different kinds ways you can write to a <see cref="FileSystemWritableFileStream"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#enumdef-writecommandtype">See the API definition here</see>.</remarks>
[JsonConverter(typeof(WriteCommandTypeConverter))]
public enum WriteCommandType
{
    /// <summary>
    /// With this command type you can write to a file stream.
    /// </summary>
    Write,

    /// <summary>
    /// With this command type you can move the cursor inside a file stream.
    /// </summary>
    Seek,

    /// <summary>
    /// With this command type you can truncate the size of the file that the file stream points to.
    /// </summary>
    Truncate,
}
