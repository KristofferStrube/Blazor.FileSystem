using KristofferStrube.Blazor.WebIDL;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A handle for a file or a directory in a file system.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemhandle">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public interface IFileSystemHandle : IJSWrapper
{
    /// <summary>
    /// Is <see cref="FileSystemHandleKind.File"/> if the handle is a <see cref="FileSystemFileHandle"/>, or <see cref="FileSystemHandleKind.Directory"/> if the handle is a <see cref="FileSystemDirectoryHandle"/>.
    /// </summary>
    public Task<FileSystemHandleKind> GetKindAsync();

    /// <summary>
    /// The last path component of the handles locator's path.
    /// </summary>
    public Task<string> GetNameAsync();

    /// <summary>
    /// Checks if <see langword="this"/> handle and the <paramref name="other"/> handle are the same.
    /// </summary>
    /// <param name="other">Some other handle</param>
    /// <returns><see langword="true"/> if <see langword="this"/> handle is the same as the <paramref name="other"/> handle; else <see langword="false"/></returns>
    public Task<bool> IsSameEntryAsync(IFileSystemHandle other);
}