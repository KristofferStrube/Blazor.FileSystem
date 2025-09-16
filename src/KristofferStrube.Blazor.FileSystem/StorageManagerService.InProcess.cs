using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IStorageManagerServiceInProcess"/>
public class StorageManagerServiceInProcess : StorageManagerService, IStorageManagerServiceInProcess
{
    /// <summary>
    /// Creates the service.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    public StorageManagerServiceInProcess(IJSRuntime jSRuntime) : base(jSRuntime) { }

    /// <inheritdoc/>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc/>
    public new async Task<FileSystemDirectoryHandleInProcess> GetOriginPrivateDirectoryAsync(FileSystemOptions options)
    {
        IJSInProcessObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("navigator.storage.getDirectory");
        return await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, directoryHandle, options, new CreationOptions()
        {
            DisposesJSReference = true
        });
    }
}
