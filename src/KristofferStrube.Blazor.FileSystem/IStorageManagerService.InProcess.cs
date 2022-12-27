using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem
{
    public interface IStorageManagerServiceInProcess :
        IStorageManagerService<
            FileSystemFileHandleInProcess,
            FileSystemDirectoryHandleInProcess,
            IJSInProcessObjectReference>
    {
        new Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync();
    }
}