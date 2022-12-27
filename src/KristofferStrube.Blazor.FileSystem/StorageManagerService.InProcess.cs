using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

public class StorageManagerServiceInProcess :
    BaseStorageManagerService<
        FileSystemFileHandleInProcess,
        FileSystemDirectoryHandleInProcess,
        IJSInProcessObjectReference>,
    IStorageManagerServiceInProcess, IStorageManagerService
{
    public StorageManagerServiceInProcess(IJSRuntime jSRuntime) : base(jSRuntime)
    {
    }

    protected override async Task<FileSystemDirectoryHandleInProcess> CreateDirectoryHandleAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        return await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, jSReference, options);
    }

    protected override async Task<FileSystemFileHandleInProcess> CreateFileHandleAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        return await FileSystemFileHandleInProcess.CreateAsync(jSRuntime, jSReference, options);
    }

    async Task<FileSystemDirectoryHandle> IStorageManagerService<FileSystemFileHandle, FileSystemDirectoryHandle, IJSObjectReference>.GetOriginPrivateDirectoryAsync()
    {
        return await this.GetOriginPrivateDirectoryAsync();
    }
}
