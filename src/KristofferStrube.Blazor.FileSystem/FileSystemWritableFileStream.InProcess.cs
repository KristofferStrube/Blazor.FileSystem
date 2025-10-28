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
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<FileSystemWritableFileStreamInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new FileSystemWritableFileStreamInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected FileSystemWritableFileStreamInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
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
