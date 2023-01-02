using KristofferStrube.Blazor.FileSystem.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

public class StorageManagerService : IStorageManagerService
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    protected readonly IJSRuntime jSRuntime;

    public StorageManagerService(IJSRuntime jSRuntime)
    {
        helperTask = new(() => jSRuntime.GetHelperAsync(FileSystemOptions.DefaultInstance));
        this.jSRuntime = jSRuntime;
    }

    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public async Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public async Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync(FileSystemOptions options)
    {
        IJSObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSObjectReference>("navigator.storage.getDirectory");
        return new FileSystemDirectoryHandle(jSRuntime, directoryHandle, options);
    }

    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
