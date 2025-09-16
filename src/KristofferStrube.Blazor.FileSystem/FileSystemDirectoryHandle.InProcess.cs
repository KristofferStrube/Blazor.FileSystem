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

    /// <inheritdoc cref="FileSystemHandleInProcess.inProcessHelper"/>
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

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemDirectoryHandleInProcess"/> with options for where the JS helper module will be found at.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">Options for what path the JS helper module will be found at.</param>
    public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
        return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSReference, options, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemDirectoryHandleInProcess"/> with options for where the JS helper module will be found at and whether its JS reference should be disposed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="fileSystemOptions">Options for what path the JS helper module will be found at.</param>
    /// <param name="creationOptions">Options for what path the JS helper module will be found at.</param>
    public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions creationOptions)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(fileSystemOptions);
        return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSReference, fileSystemOptions, creationOptions);
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

    /// <inheritdoc cref="FileSystemDirectoryHandle.ValuesAsync"/>
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
                        fileSystemOptions);

                    FileSystemHandleKind kind = await fileSystemHandle.GetKindAsync();

                    if (kind is FileSystemHandleKind.File)
                    {
                        return await FileSystemFileHandleInProcess.CreateAsync(JSRuntime, fileSystemHandle.JSReference, fileSystemOptions, new CreationOptions()
                        {
                            DisposesJSReference = true
                        });
                    }
                    else
                    {
                        return await CreateAsync(JSRuntime, fileSystemHandle.JSReference, new CreationOptions()
                        {
                            DisposesJSReference = true
                        });
                    }
                }
                )
                .ToArray()
        );
    }

    /// <inheritdoc cref="FileSystemDirectoryHandle.GetFileHandleAsync(string, FileSystemGetFileOptions?)"/>
    public new async Task<FileSystemFileHandleInProcess> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
    {
        IJSInProcessObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getFileHandle", name, options);
        return await FileSystemFileHandleInProcess.CreateAsync(JSRuntime, jSFileSystemFileHandle, fileSystemOptions, new CreationOptions()
        {
            DisposesJSReference = true
        });
    }

    /// <inheritdoc cref="FileSystemDirectoryHandle.GetDirectoryHandleAsync(string, FileSystemGetDirectoryOptions?)"/>
    public new async Task<FileSystemDirectoryHandleInProcess> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
    {
        IJSInProcessObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getDirectoryHandle", name, options);
        return await CreateAsync(JSRuntime, jSFileSystemFileHandle, fileSystemOptions, new CreationOptions()
        {
            DisposesJSReference = true
        });
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await inProcessHelper.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
