using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href="https://fs.spec.whatwg.org/#filesystemfilehandle">FileSystemFileHandle browser specs</see>
/// </summary>
public class FileSystemFileHandle : FileSystemHandle
{
    public static new FileSystemFileHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Create(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    public static new FileSystemFileHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return new(jSRuntime, jSReference, options);
    }

    internal FileSystemFileHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options) : base(jSRuntime, jSReference, options) { }

    public async Task<FileAPI.File> GetFileAsync()
    {
        IJSObjectReference jSFile = await JSReference.InvokeAsync<IJSObjectReference>("getFile");
        return FileAPI.File.Create(jSRuntime, jSFile);
    }

    public async Task<FileSystemWritableFileStream> CreateWritableAsync(FileSystemCreateWritableOptions? fileSystemCreateWritableOptions = null)
    {
        IJSObjectReference jSFileSystemWritableFileStream = await JSReference.InvokeAsync<IJSObjectReference>("createWritable", fileSystemCreateWritableOptions);
        return await FileSystemWritableFileStream.CreateAsync(jSRuntime, jSFileSystemWritableFileStream);
    }
}
