using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem
{
    public interface IStorageManagerService<TFsFileHandle, TFsDirectoryHandle, TObjReference> : IAsyncDisposable
        where TFsFileHandle : FileSystemFileHandle
        where TFsDirectoryHandle : FileSystemDirectoryHandle
        where TObjReference : IJSObjectReference
    {
        Task<TFsDirectoryHandle> GetOriginPrivateDirectoryAsync();
    }

    public interface IStorageManagerService : IStorageManagerService<FileSystemFileHandle, FileSystemDirectoryHandle, IJSObjectReference>
    {
    }
}