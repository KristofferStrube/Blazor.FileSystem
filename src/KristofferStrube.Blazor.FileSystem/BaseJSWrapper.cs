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
    /// A lazily evaluated JS module that gives access to helper methods.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <summary>
    /// Options for where the helper JS module is located.
    /// </summary>
    protected readonly FileSystemOptions fileSystemOptions;

    /// <summary>
    /// Options for where the helper JS module is located.
    /// </summary>
    [Obsolete("This is here for backwards compatibility. It was replaced by 'fileSystemOptions' as 'options' was ambiguous.")]
    protected readonly FileSystemOptions options;

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc cref="IJSCreatable{T}.CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    internal BaseJSWrapper(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options)
    {
        this.fileSystemOptions = fileSystemOptions;
#pragma warning disable CS0618 // Type or member is obsolete
        this.options = fileSystemOptions;
#pragma warning restore CS0618 // Type or member is obsolete
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
