namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A service that exposes methods for getting an in-process version of a Origin Private Directory.
/// </summary>
public interface IStorageManagerServiceInProcess : IStorageManagerService
{
    /// <inheritdoc cref="IStorageManagerService.GetOriginPrivateDirectoryAsync()"/>
    public new Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync();
}