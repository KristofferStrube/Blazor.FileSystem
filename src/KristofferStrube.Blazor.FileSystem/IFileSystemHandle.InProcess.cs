namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// An in-process version of a handle for a file or a directory in a file system.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemhandle">See the API definition here</see>.</remarks>
public interface IFileSystemHandleInProcess : IFileSystemHandle
{
    /// <inheritdoc cref="FileSystemHandle.GetKindAsync"/>
    public FileSystemHandleKind Kind { get; }

    /// <inheritdoc cref="FileSystemHandle.GetNameAsync"/>
    public string Name { get; }
}