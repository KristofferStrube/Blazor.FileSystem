using KristofferStrube.Blazor.FileSystem.Extensions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;
public abstract class BaseStorageManagerService<TFsFileHandle, TFsDirectoryHandle, TObjReference> : IStorageManagerService<TFsFileHandle, TFsDirectoryHandle, TObjReference>
    where TFsFileHandle : FileSystemFileHandle
    where TFsDirectoryHandle : FileSystemDirectoryHandle
    where TObjReference : IJSObjectReference
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    protected readonly IJSRuntime jSRuntime;

    public BaseStorageManagerService(IJSRuntime jSRuntime)
    {
        helperTask = new(() => jSRuntime.GetHelperAsync(FileSystemOptions.DefaultInstance));
        this.jSRuntime = jSRuntime;
    }

    #region GetOriginPrivateDirectoryAsync

    /// <summary>
    /// <see href="https://wicg.github.io/file-system-access/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public async Task<TFsDirectoryHandle> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <summary>
    /// <see href="https://wicg.github.io/file-system-access/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public async Task<TFsDirectoryHandle> GetOriginPrivateDirectoryAsync(FileSystemOptions fasOptions)
    {
        TObjReference jSFileHandle = await jSRuntime.InvokeAsync<TObjReference>("navigator.storage.getDirectory");
        return await CreateDirectoryHandleAsync(jSRuntime, jSFileHandle, fasOptions);
    }

    #endregion

    #region Common Handling Methods

    protected abstract Task<TFsFileHandle> CreateFileHandleAsync(IJSRuntime jSRuntime, TObjReference jSReference, FileSystemOptions options);
    protected abstract Task<TFsDirectoryHandle> CreateDirectoryHandleAsync(IJSRuntime jSRuntime, TObjReference jSReference, FileSystemOptions options);

    #endregion

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
