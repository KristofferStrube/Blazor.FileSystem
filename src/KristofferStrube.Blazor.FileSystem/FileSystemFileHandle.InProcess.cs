using KristofferStrube.Blazor.DOM.Extensions;
using KristofferStrube.Blazor.FileAPI;
using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A in-process version of a <see cref="FileSystemHandle"/> that represents a file.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemfilehandle">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemFileHandleInProcess : FileSystemFileHandle, IFileSystemHandleInProcess, IJSInProcessCreatable<FileSystemFileHandleInProcess, FileSystemFileHandle>
{
    /// <inheritdoc cref="IJSInProcessCreatable{TInProcess, T}.JSReference"/>
    public new IJSInProcessObjectReference JSReference { get; set; }

    /// <summary>
    /// A lazily evaluated task that gives access to helper methods.
    /// </summary>
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <inheritdoc/>
    public static async Task<FileSystemFileHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference);
    }

    /// <inheritdoc/>
    public static async Task<FileSystemFileHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(FileSystemOptions.DefaultInstance);
        return new FileSystemFileHandleInProcess(jSRuntime, inProcessHelper, jSReference, FileSystemOptions.DefaultInstance, options);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    public static async Task<FileSystemFileHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemFileHandleInProcess(jSRuntime, inProcessHelper, jSReference, options, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected FileSystemFileHandleInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc cref="IFileSystemHandle.GetKindAsync"/>
    public FileSystemHandleKind Kind => inProcessHelper.Invoke<FileSystemHandleKind>("getAttribute", JSReference, "kind");

    /// <inheritdoc cref="IFileSystemHandle.GetNameAsync"/>
    public string Name => inProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

    /// <inheritdoc cref="FileSystemFileHandle.GetFileAsync"/>
    public new async Task<FileInProcess> GetFileAsync()
    {
        IJSInProcessObjectReference jSFile = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getFile");
        return await FileInProcess.CreateAsync(JSRuntime, jSFile);
    }

    /// <inheritdoc cref="FileSystemFileHandle.CreateWritableAsync"/>
    public new async Task<FileSystemWritableFileStreamInProcess> CreateWritableAsync(FileSystemCreateWritableOptions? fileSystemCreateWritableOptions = null)
    {
        IJSInProcessObjectReference jSFileSystemWritableFileStream = await JSReference.InvokeAsync<IJSInProcessObjectReference>("createWritable", fileSystemCreateWritableOptions);
        return new FileSystemWritableFileStreamInProcess(JSRuntime, inProcessHelper, jSFileSystemWritableFileStream, new() { DisposesJSReference = true });
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await inProcessHelper.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
