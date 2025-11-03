namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A service that exposes methods for getting the Origin Private Directory
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#ref-for-storagemanager">See the API definition here</see>.</remarks>
public interface IStorageManagerService
{
    /// <summary>
    /// Gets a <see cref="FileSystemDirectoryHandle"/> that is a local bucket for the current domain. It is persistent across sessions.
    /// </summary>
    public Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync();
}