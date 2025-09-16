namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A service that exposes methods for getting an in-process version of a Origin Private Directory.
/// </summary>
public interface IStorageManagerServiceInProcess : IStorageManagerService
{
    /// <summary>
    /// Gets a <see cref="FileSystemDirectoryHandleInProcess"/> that is a local bucket for the current domain. It is persistent across sessions.
    /// </summary>
    public new Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync();

    /// <summary>
    /// <inheritdoc cref="GetOriginPrivateDirectoryAsync()" path="/summary"/>
    /// This overload has explicit options for how you want to import the JS helper module used in the library.
    /// </summary>
    /// <param name="options">Options for how the wrapper should construct its JS helper module</param>
    public new Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync(FileSystemOptions options);
}