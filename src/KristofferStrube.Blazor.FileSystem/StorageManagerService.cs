using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IStorageManagerService"/>
public class StorageManagerService : IStorageManagerService, IAsyncDisposable
{
    /// <inheritdoc cref="BaseJSWrapper.helperTask"/>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

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
        helperTask = new(() => jSRuntime.GetHelperAsync(FileSystemOptions.DefaultInstance));
        this.jSRuntime = jSRuntime;
    }

    /// <inheritdoc/>
    public async Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync()
    {
        return await GetOriginPrivateDirectoryAsync(FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc/>
    public async Task<FileSystemDirectoryHandle> GetOriginPrivateDirectoryAsync(FileSystemOptions options)
    {
        IJSObjectReference directoryHandle = await jSRuntime.InvokeAsync<IJSObjectReference>("navigator.storage.getDirectory");
        return await FileSystemDirectoryHandle.CreateAsync(jSRuntime, directoryHandle, options, new CreationOptions()
        {
            DisposesJSReference = true
        });
    }

    /// <inheritdoc/>
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
