using KristofferStrube.Blazor.FileAPI;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// Write params that can be used to write a <see cref="Blob"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-writeparams">See the API definition here</see>.</remarks>
public class BlobWriteParams : BaseWriteParams
{
    /// <summary>
    /// Creates the write params.
    /// </summary>
    /// <param name="type">The type of write that will be performed. The data of the write params will only be used if the <see cref="WriteCommandType.Write"/> is chosen.</param>
    public BlobWriteParams(WriteCommandType type)
    {
        Type = type;
    }

    /// <summary>
    /// The data that will be written.
    /// </summary>
    [JsonPropertyName("data")]
    public Blob? Data { get; set; }
}

/// <summary>
/// Write params that can be used to write a <see cref="string"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-writeparams">See the API definition here</see>.</remarks>
public class StringWriteParams : BaseWriteParams
{
    /// <inheritdoc cref="BlobWriteParams(WriteCommandType)"/>
    public StringWriteParams(WriteCommandType type)
    {
        Type = type;
    }

    /// <inheritdoc cref="BlobWriteParams.Data"/>
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

/// <summary>
/// Write params that can be used to write bytes in a byte-array.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-writeparams">See the API definition here</see>.</remarks>
public class ByteArrayWriteParams : BaseWriteParams
{
    /// <inheritdoc cref="BlobWriteParams(WriteCommandType)"/>
    public ByteArrayWriteParams(WriteCommandType type)
    {
        Type = type;
    }

    /// <inheritdoc cref="BlobWriteParams.Data"/>
    [JsonPropertyName("data")]
    public byte[]? Data { get; set; }
}

/// <summary>
/// A base class for the different kinds of write params.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-writeparams">See the API definition here</see>.</remarks>
public abstract class BaseWriteParams
{
    /// <summary>
    /// The type write that will be performed.
    /// </summary>
    [JsonPropertyName("type")]
    public WriteCommandType Type { get; init; }

    /// <summary>
    /// If <see cref="WriteCommandType.Truncate"/> is chosen for the <see cref="Type"/> then this is the new size of the underlying file.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("size")]
    public ulong? Size { get; set; }

    /// <summary>
    /// If <see cref="WriteCommandType.Write"/> is chosen for the <see cref="Type"/> then the data will be written at this offset in the file.<br />
    /// And if <see cref="WriteCommandType.Seek"/> is chosen for the <see cref="Type"/> then this position will be new position of the cursor into the file.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("position")]
    public ulong? Position { get; set; }
}