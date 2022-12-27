using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

public class StorageManagerService :
    BaseStorageManagerService<
        FileSystemFileHandle,
        FileSystemDirectoryHandle,
        IJSObjectReference>,
    IStorageManagerService
{
    public StorageManagerService(IJSRuntime jSRuntime) : base(jSRuntime)
    {
    }

    protected override Task<FileSystemDirectoryHandle> CreateDirectoryHandleAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return Task.FromResult(new FileSystemDirectoryHandle(jSRuntime, jSReference, options));
    }

    protected override Task<FileSystemFileHandle> CreateFileHandleAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return Task.FromResult(new FileSystemFileHandle(jSRuntime, jSReference, options));
    }
}
