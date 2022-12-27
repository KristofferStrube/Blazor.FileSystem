using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

public class StorageManagerServiceInProcess : StorageManagerService, IStorageManagerServiceInProcess
{
    public StorageManagerServiceInProcess(IJSRuntime jSRuntime) : base(jSRuntime) { }

    /// <summary>
    /// <see href="https://wicg.github.io/file-system-access/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <summary>
    /// <see href="https://wicg.github.io/file-system-access/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync(FileSystemOptions options)
    {
        IJSInProcessObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("navigator.storage.getDirectory");
        return await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, directoryHandle, options);
    }
}
