using KristofferStrube.Blazor.FileAPI;
using KristofferStrube.Blazor.FileSystem.Extensions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href=https://fs.spec.whatwg.org/#filesystemfilehandle">FileSystemFileHandle browser specs</see>
/// </summary>
public class FileSystemFileHandleInProcess : FileSystemFileHandle
{
    public new IJSInProcessObjectReference JSReference;
    protected readonly IJSInProcessObjectReference inProcessHelper;

    public static async Task<FileSystemFileHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    public static async Task<FileSystemFileHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemFileHandleInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    internal FileSystemFileHandleInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions options) : base(jSRuntime, jSReference, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    public FileSystemHandleKind Kind => inProcessHelper.Invoke<FileSystemHandleKind>("getAttribute", JSReference, "kind");

    public string Name => inProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

    public new async Task<FileInProcess> GetFileAsync()
    {
        IJSInProcessObjectReference jSFile = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getFile");
        return await FileInProcess.CreateAsync(jSRuntime, jSFile);
    }

    public new async Task<FileSystemWritableFileStreamInProcess> CreateWritableAsync(FileSystemCreateWritableOptions? fileSystemCreateWritableOptions = null)
    {
        IJSInProcessObjectReference jSFileSystemWritableFileStream = await JSReference.InvokeAsync<IJSInProcessObjectReference>("createWritable", fileSystemCreateWritableOptions);
        return new FileSystemWritableFileStreamInProcess(jSRuntime, inProcessHelper, jSFileSystemWritableFileStream);
    }
}
