using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// Base class for wrapping objects in the Blazor.FileSystem library.
/// </summary>
public abstract class BaseJSWrapper : IAsyncDisposable, IJSWrapper
{
    /// <summary>
    /// A lazily evaluated JS module that gives access to helper methods for the File System API.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc cref="IJSCreatable{T}.CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    internal BaseJSWrapper(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSReference = jSReference;
        JSRuntime = jSRuntime;
        DisposesJSReference = options.DisposesJSReference;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}
