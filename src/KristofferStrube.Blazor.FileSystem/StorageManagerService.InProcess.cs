using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IStorageManagerService"/>
public class StorageManagerServiceInProcess : StorageManagerService, IStorageManagerServiceInProcess
{
    /// <summary>
    /// Creates a <see cref="StorageManagerServiceInProcess"/>
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    public StorageManagerServiceInProcess(IJSRuntime jSRuntime) : base(jSRuntime) { }

    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#dom-storagemanager-getdirectory">getDirectory() for StorageManager browser specs</see>
    /// </summary>
    /// <returns></returns>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync(FileSystemOptions options)
    {
        IJSInProcessObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("navigator.storage.getDirectory");
        return await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, directoryHandle, options);
    }
}
