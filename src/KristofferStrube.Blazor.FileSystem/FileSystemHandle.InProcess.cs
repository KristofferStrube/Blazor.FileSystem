using KristofferStrube.Blazor.DOM.Extensions;
using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IFileSystemHandleInProcess"/>
[IJSWrapperConverter]
public class FileSystemHandleInProcess : FileSystemHandle, IFileSystemHandleInProcess, IJSInProcessCreatable<FileSystemHandleInProcess, FileSystemHandle>
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; set; }

    /// <summary>
    /// A lazily evaluated task that gives access to helper methods.
    /// </summary>
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <inheritdoc/>
    public static async Task<FileSystemHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new CreationOptions());
    }

    /// <inheritdoc/>
    public static async Task<FileSystemHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(FileSystemOptions.DefaultInstance);
        return new FileSystemHandleInProcess(jSRuntime, inProcessHelper, jSReference, FileSystemOptions.DefaultInstance, options);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)" />
    public static async Task<FileSystemHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemHandleInProcess(jSRuntime, inProcessHelper, jSReference, options, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)" />
    protected FileSystemHandleInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public FileSystemHandleKind Kind => inProcessHelper.Invoke<FileSystemHandleKind>("getAttribute", JSReference, "kind");

    /// <inheritdoc cref="FileSystemHandle.GetNameAsync"/>
    public string Name => inProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await inProcessHelper.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
