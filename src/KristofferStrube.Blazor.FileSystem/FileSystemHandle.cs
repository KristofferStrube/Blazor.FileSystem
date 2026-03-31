using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <inheritdoc cref="IFileSystemHandle"/>
[IJSWrapperConverter]
public class FileSystemHandle : BaseJSWrapper, IFileSystemHandle, IJSCreatable<FileSystemHandle>
{
    /// <inheritdoc/>
    public static async Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        await using ValueReference handle = new(jSRuntime, jSReference, null, new() { DisposesJSReference = false });

        if (jSReference is IJSInProcessObjectReference inProcessObjectReference)
        {
            handle.ValueMapper = new()
            {
                ["filesystemdirectoryhandle"] = async () => await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, inProcessObjectReference, new() { DisposesJSReference = true }),
                ["filesystemfilehandle"] = async () => await FileSystemFileHandleInProcess.CreateAsync(jSRuntime, inProcessObjectReference, new() { DisposesJSReference = true }),
            };
        }
        else
        {
            handle.ValueMapper = new()
            {
                ["filesystemdirectoryhandle"] = async () => await FileSystemDirectoryHandle.CreateAsync(jSRuntime, jSReference, new() { DisposesJSReference = true }),
                ["filesystemfilehandle"] = async () => await FileSystemFileHandle.CreateAsync(jSRuntime, jSReference, new() { DisposesJSReference = true }),
            };
        }

        return (FileSystemHandle)(await handle.GetValueAsync())!;
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileSystemHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

    /// <inheritdoc/>
    public async Task<FileSystemHandleKind> GetKindAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<FileSystemHandleKind>("getAttribute", JSReference, "kind");
    }

    /// <inheritdoc/>
    public async Task<string> GetNameAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("getAttribute", JSReference, "name");
    }

    /// <inheritdoc/>
    public async Task<bool> IsSameEntryAsync(IFileSystemHandle other)
    {
        return await JSReference.InvokeAsync<bool>("isSameEntry", other.JSReference);
    }
}
