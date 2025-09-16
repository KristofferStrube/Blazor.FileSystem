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

    /// <summary>
    /// <inheritdoc cref="GetOriginPrivateDirectoryAsync()" path="/summary"/>
    /// This overload has explicit options for how you want to import the JS helper module used in the library.
    /// </summary>
    /// <param name="options">Options for how the wrapper should construct its JS helper module</param>
    public Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync(FileSystemOptions options);
}