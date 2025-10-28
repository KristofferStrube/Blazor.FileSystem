using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IStorageManagerService"/>
public class StorageManagerService : IStorageManagerService
{
    /// <summary>
    /// The <see cref="JSRuntime"/> used for making JSInterop calls.
    /// </summary>
    protected readonly IJSRuntime jSRuntime;

    /// <summary>
    /// Creates the service. Should be a scoped service, especially when used in Blazor Server render mode.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    public StorageManagerService(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    /// <inheritdoc/>
    public async Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync()
    {
        IJSObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSObjectReference>("navigator.storage.getDirectory");
        return await FileSystemDirectoryHandle.CreateAsync(jSRuntime, directoryHandle, new CreationOptions()
        {
            DisposesJSReference = true
        });
    }
}
