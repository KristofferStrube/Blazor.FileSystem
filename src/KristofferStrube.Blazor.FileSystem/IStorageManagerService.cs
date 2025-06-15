namespace KristofferStrube.Blazor.FileSystem
{
    public interface IStorageManagerService
    {
        Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync();
    }
}