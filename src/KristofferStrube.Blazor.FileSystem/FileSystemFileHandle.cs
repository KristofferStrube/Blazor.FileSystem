using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;
using File = KristofferStrube.Blazor.FileAPI.File;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A <see cref="FileSystemHandle"/> that represents a file.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemfilehandle">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemFileHandle : FileSystemHandle, IJSCreatable<FileSystemFileHandle>
{
    /// <inheritdoc/>
    public static new async Task<FileSystemFileHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static new Task<FileSystemFileHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new FileSystemFileHandle(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)" path="/summary"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static new FileSystemFileHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new(jSRuntime, jSReference, new());
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileSystemFileHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

    /// <summary>
    /// Returns a <see cref="File"/> that that represents the state of the file on the disk. If the file on disk changes or is removed after this method has returned the file then this <see cref="File"/> will likely no longer be readable.
    /// </summary>
    public async Task<File> GetFileAsync()
    {
        IJSObjectReference jSFile = await JSReference.InvokeAsync<IJSObjectReference>("getFile");
        return await File.CreateAsync(JSRuntime, jSFile, new()
        {
            DisposesJSReference = true
        });
    }

    /// <summary>
    /// Returns a <see cref="FileSystemWritableFileStream"/> that can be used to write to the file.
    /// Any changes made through stream won’t be reflected in the file entry locatable by fileHandle’s locator until the stream has been closed (or disposed).
    /// User agents try to ensure that no partial writes happen, i.e. the file will either contain its old contents or it will contain whatever data was written through stream up until the stream has been closed (or disposed).
    /// This is typically implemented by writing data to a temporary file, and only replacing the file entry locatable by fileHandle’s locator with the temporary file when the writable filestream is closed (or disposed).
    /// </summary>
    /// <param name="fileSystemCreateWritableOptions">
    /// If <see cref="FileSystemCreateWritableOptions.KeepExistingData"/> is <see langword="false"/> or not specified, the temporary file starts out empty, otherwise the existing file is first copied to this temporary file.
    /// </param>
    public async Task<FileSystemWritableFileStream> CreateWritableAsync(FileSystemCreateWritableOptions? fileSystemCreateWritableOptions = null)
    {
        IJSObjectReference jSFileSystemWritableFileStream = await JSReference.InvokeAsync<IJSObjectReference>("createWritable", fileSystemCreateWritableOptions);
        return await FileSystemWritableFileStream.CreateAsync(JSRuntime, jSFileSystemWritableFileStream, new()
        {
            DisposesJSReference = true
        });
    }
}
