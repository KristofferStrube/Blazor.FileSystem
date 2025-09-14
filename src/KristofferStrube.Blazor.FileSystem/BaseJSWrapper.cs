using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// Base class for wrapping objects in the Blazor.WebAudio library.
/// </summary>
[IJSWrapperConverter]
public abstract class BaseJSWrapper : IAsyncDisposable, IJSWrapper
{
    /// <summary>
    /// A lazily evaluated task that gives access to helper methods.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <summary>
    /// Options for where it will find the script file that it needs to make complex JSInterop.
    /// </summary>
    public FileSystemOptions FileSystemOptions { get; set; }

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc cref="IJSCreatable{T}.CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected BaseJSWrapper(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options)
    {
        FileSystemOptions = fileSystemOptions;
        helperTask = new(async () => await jSRuntime.GetHelperAsync(fileSystemOptions));
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
