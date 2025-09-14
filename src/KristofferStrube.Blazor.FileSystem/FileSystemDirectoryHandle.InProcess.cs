using KristofferStrube.Blazor.DOM.Extensions;
using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// An in-process version of a <see cref="FileSystemHandle"/> that represents a directory.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemdirectoryhandle">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemDirectoryHandleInProcess : FileSystemDirectoryHandle, IFileSystemHandleInProcess, IJSInProcessCreatable<FileSystemDirectoryHandleInProcess, FileSystemDirectoryHandle>
{
    /// <inheritdoc cref="IJSInProcessCreatable{TInProcess, T}.JSReference"/>
    public new IJSInProcessObjectReference JSReference { get; set; }

    /// <summary>
    /// A lazily evaluated task that gives access to helper methods.
    /// </summary>
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <inheritdoc/>
    public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new CreationOptions());
    }

    /// <inheritdoc/>
    public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(FileSystemOptions.DefaultInstance);
        return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSReference, FileSystemOptions.DefaultInstance, options);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)" />
    public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSReference, options, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)" />
    protected FileSystemDirectoryHandleInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc cref="IFileSystemHandle.GetKindAsync"/>
    public FileSystemHandleKind Kind => inProcessHelper.Invoke<FileSystemHandleKind>("getAttribute", JSReference, "kind");

    /// <inheritdoc cref="IFileSystemHandle.GetNameAsync"/>
    public string Name => inProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

    /// <inheritdoc/>
    public new async Task<IFileSystemHandleInProcess[]> ValuesAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        IJSObjectReference jSValues = await JSReference.InvokeAsync<IJSObjectReference>("values");
        IJSObjectReference jSEntries = await helper.InvokeAsync<IJSObjectReference>("arrayFrom", jSValues);
        int length = await helper.InvokeAsync<int>("size", jSEntries);
        return await Task.WhenAll(
            Enumerable
                .Range(0, length)
                .Select<int, Task<IFileSystemHandleInProcess>>(async i =>
                {
                    FileSystemHandleInProcess fileSystemHandle = await FileSystemHandleInProcess.CreateAsync(
                        JSRuntime,
                        await jSEntries.InvokeAsync<IJSInProcessObjectReference>("at", i),
                        FileSystemOptions);
                    return (fileSystemHandle.Kind is FileSystemHandleKind.File)
                        ? await FileSystemFileHandleInProcess.CreateAsync(JSRuntime, fileSystemHandle.JSReference, FileSystemOptions)
                        : await FileSystemDirectoryHandleInProcess.CreateAsync(JSRuntime, fileSystemHandle.JSReference, FileSystemOptions);
                }
                )
                .ToArray()
        );
    }

    /// <inheritdoc cref="FileSystemDirectoryHandle.GetFileHandleAsync(string, FileSystemGetFileOptions?)"/>
    public new async Task<FileSystemFileHandleInProcess> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
    {
        IJSInProcessObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getFileHandle", name, options);
        return await FileSystemFileHandleInProcess.CreateAsync(JSRuntime, jSFileSystemFileHandle, FileSystemOptions);
    }

    /// <inheritdoc cref="FileSystemDirectoryHandle.GetDirectoryHandleAsync(string, FileSystemGetDirectoryOptions?)"/>
    public new async Task<FileSystemDirectoryHandleInProcess> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
    {
        IJSInProcessObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getDirectoryHandle", name, options);
        return await CreateAsync(JSRuntime, jSFileSystemFileHandle, FileSystemOptions);
    }
}
