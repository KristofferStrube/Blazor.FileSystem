namespace KristofferStrube.Blazor.FileSystem;

public interface IStorageManagerServiceInProcess : IStorageManagerService
{
    new Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync();
}