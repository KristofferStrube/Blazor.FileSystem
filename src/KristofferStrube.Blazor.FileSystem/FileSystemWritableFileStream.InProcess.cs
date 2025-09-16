using KristofferStrube.Blazor.DOM.Extensions;
using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.Streams;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// An in-process <see cref="WritableStream"/> that can write to and seek in a <see cref="File"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#filesystemwritablefilestream">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemWritableFileStreamInProcess : FileSystemWritableFileStream, IJSInProcessCreatable<FileSystemWritableFileStreamInProcess, FileSystemWritableFileStream>
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <inheritdoc cref="FileSystemHandleInProcess.inProcessHelper"/>
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <inheritdoc/>
    public static async Task<FileSystemWritableFileStreamInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc/>
    public static async Task<FileSystemWritableFileStreamInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(FileSystemOptions.DefaultInstance);
        return new FileSystemWritableFileStreamInProcess(jSRuntime, inProcessHelper, jSReference, FileSystemOptions.DefaultInstance, options);
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemWritableFileStreamInProcess"/> with options for where the JS helper module will be found at.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">Options for what path the JS helper module will be found at.</param>
    public static async Task<FileSystemWritableFileStreamInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemWritableFileStreamInProcess(jSRuntime, inProcessHelper, jSReference, options, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemWritableFileStreamInProcess"/> with options for where the JS helper module will be found at and whether its JS reference should be disposed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="fileSystemOptions">Options for what path the JS helper module will be found at.</param>
    /// <param name="creationOptions">Options for what path the JS helper module will be found at.</param>
    public static async Task<FileSystemWritableFileStreamInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions creationOptions)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(fileSystemOptions);
        return new FileSystemWritableFileStreamInProcess(jSRuntime, inProcessHelper, jSReference, fileSystemOptions, creationOptions);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected FileSystemWritableFileStreamInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await inProcessHelper.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
