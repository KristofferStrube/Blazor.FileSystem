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
}