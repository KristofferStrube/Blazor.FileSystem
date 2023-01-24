using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// <see href="https://fs.spec.whatwg.org/#filesystemdirectoryhandle">FileSystemDirectoryHandle browser specs</see>
/// </summary>
public class FileSystemDirectoryHandle : FileSystemHandle
{
    public static new FileSystemDirectoryHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Create(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    public static new FileSystemDirectoryHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return new(jSRuntime, jSReference, options);
    }

    internal FileSystemDirectoryHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options) : base(jSRuntime, jSReference, options) { }

    public async Task<IFileSystemHandle[]> ValuesAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        IJSObjectReference jSValues = await JSReference.InvokeAsync<IJSObjectReference>("values");
        IJSObjectReference jSEntries = await helper.InvokeAsync<IJSObjectReference>("arrayFrom", jSValues);
        int length = await helper.InvokeAsync<int>("size", jSEntries);
        return await Task.WhenAll(
            Enumerable
                .Range(0, length)
                .Select<int, Task<FileSystemHandle>>(async i =>
                    {
                        var fileSystemHandle = new FileSystemHandle(
                            jSRuntime,
                            await jSEntries.InvokeAsync<IJSObjectReference>("at", i),
                            options);
                        return (await fileSystemHandle.GetKindAsync() is FileSystemHandleKind.File)
                            ? FileSystemFileHandle.Create(jSRuntime, fileSystemHandle.JSReference, options)
                            : FileSystemDirectoryHandle.Create(jSRuntime, fileSystemHandle.JSReference, options);
                    }
                )
                .ToArray()
        );
    }

    public async Task<FileSystemFileHandle> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
    {
        IJSObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSObjectReference>("getFileHandle", name, options);
        return new FileSystemFileHandle(jSRuntime, jSFileSystemFileHandle, this.options);
    }

    public async Task<FileSystemDirectoryHandle> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
    {
        IJSObjectReference jSFileSystemDirectoryHandle = await JSReference.InvokeAsync<IJSObjectReference>("getDirectoryHandle", name, options);
        return new FileSystemDirectoryHandle(jSRuntime, jSFileSystemDirectoryHandle, this.options);
    }

    public async Task RemoveEntryAsync(string name, FileSystemRemoveOptions? options = null)
    {
        await JSReference.InvokeVoidAsync("removeEntry", name, options);
    }

    public async Task<string[]?> ResolveAsync(IFileSystemHandle possibleDescendant)
    {
        return await JSReference.InvokeAsync<string[]?>("resolve", possibleDescendant.JSReference);
    }
}
