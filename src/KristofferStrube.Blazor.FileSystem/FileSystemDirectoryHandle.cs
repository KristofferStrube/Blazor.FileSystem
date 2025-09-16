using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A <see cref="FileSystemHandle"/> that represents a directory.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemdirectoryhandle">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemDirectoryHandle : FileSystemHandle, IJSCreatable<FileSystemDirectoryHandle>
{
    /// <inheritdoc/>
    public static new async Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc/>
    public static new Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new FileSystemDirectoryHandle(jSRuntime, jSReference, FileSystemOptions.DefaultInstance, options));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemDirectoryHandle"/> with options for where the JS helper module will be found at.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">Options for what path the JS helper module will be found at.</param>
    public static new Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return Task.FromResult(new FileSystemDirectoryHandle(jSRuntime, jSReference, options, new() { DisposesJSReference = true }));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemDirectoryHandle"/> with options for where the JS helper module will be found at and whether its JS reference should be disposed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="fileSystemOptions">Options for what path the JS helper module will be found at.</param>
    /// <param name="creationOptions">Options for what path the JS helper module will be found at.</param>
    public static new Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions creationOptions)
    {
        return Task.FromResult(new FileSystemDirectoryHandle(jSRuntime, jSReference, fileSystemOptions, creationOptions));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static new FileSystemDirectoryHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Create(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static new FileSystemDirectoryHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return new(jSRuntime, jSReference, options, new());
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileSystemDirectoryHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options) { }

    /// <summary>
    /// Gets all <see cref="FileSystemHandle"/>s in the directory.
    /// </summary>
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
                        await using FileSystemHandle fileSystemHandle = await FileSystemHandle.CreateAsync(
                            JSRuntime,
                            await jSEntries.InvokeAsync<IJSObjectReference>("at", i), fileSystemOptions);

                        FileSystemHandleKind kind = await fileSystemHandle.GetKindAsync();

                        if (kind is FileSystemHandleKind.File)
                        {
                            return await FileSystemFileHandle.CreateAsync(JSRuntime, fileSystemHandle.JSReference, fileSystemOptions, new CreationOptions()
                            {
                                DisposesJSReference = true
                            });
                        }
                        else
                        {
                            return await CreateAsync(JSRuntime, fileSystemHandle.JSReference, fileSystemOptions, new CreationOptions()
                            {
                                DisposesJSReference = true
                            });
                        }
                    }
                )
                .ToArray()
        );
    }

    /// <summary>
    /// Returns a handle for a file named <paramref name="name"/> in the directory entry locatable by directoryHandle’s locator.
    /// If <paramref name="options"/> defines that it should create the file then it will create it if it doesn't already exist.
    /// Creation can fail because there already is a directory with the same name, because the name uses characters that aren’t supported in file names on the underlying file system, or because the user agent for security reasons decided not to allow creation of the file.
    /// This operation requires write permission, even if the file being returned already exists.
    /// If this handle does not already have write permission, this could result in a prompt being shown to the user.
    /// To get an existing file without needing write permission, call this method with <see cref="FileSystemGetFileOptions.Create"/> set to <see langword="false"/>.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <param name="options">
    /// If <see cref="FileSystemGetFileOptions.Create"/> is <see langword="false"/> or unspecified and the file doesn't exist, then this call fails;
    /// Else if it is <see langword="true"/> then it creates the file if it can.
    /// </param>
    public async Task<FileSystemFileHandle> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
    {
        IJSObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSObjectReference>("getFileHandle", name, options);
        return await FileSystemFileHandle.CreateAsync(JSRuntime, jSFileSystemFileHandle, fileSystemOptions, new CreationOptions() { DisposesJSReference = true });
    }

    /// <summary>
    /// Returns a handle for a directory named <paramref name="name"/> in the directory entry locatable by directoryHandle’s locator .
    /// If no such directory exists, this creates a new directory. If creating the directory failed, this rejects.
    /// Creation can fail because there already is a file with the same name, or because the name uses characters that aren’t supported in directory names on the underlying file system.
    /// This operation requires write permission, even if the directory being returned already exists.
    /// If this handle does not already have write permission, this could result in a prompt being shown to the user.
    /// To get an existing directory without needing write permission, call this method with <see cref="FileSystemGetDirectoryOptions.Create"/> set to <see langword="false"/>.
    /// </summary>
    /// <param name="name">The name of the directory.</param>
    /// <param name="options">
    /// If <see cref="FileSystemGetDirectoryOptions.Create"/> is <see langword="false"/> or unspecified and the directory doesn't exist, then this call fails;
    /// Else if it is <see langword="true"/> then it creates the directory if it can.
    /// </param>
    public async Task<FileSystemDirectoryHandle> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
    {
        IJSObjectReference jSFileSystemDirectoryHandle = await JSReference.InvokeAsync<IJSObjectReference>("getDirectoryHandle", name, options);
        return new(JSRuntime, jSFileSystemDirectoryHandle, fileSystemOptions, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// If the directory entry locatable by directoryHandle’s locator contains a file named <paramref name="name"/>, or an empty directory named <paramref name="name"/>, this will attempt to delete that file or directory.
    /// Attempting to delete a file or directory that does not exist is considered success.
    /// If you attempt to delete a non-empty folder then it will fail if <paramref name="options"/> has not been set to recursively remove sub-directories.
    /// </summary>
    /// <param name="name">The name of the directory or file.</param>
    /// <param name="options">
    /// If <see cref="FileSystemRemoveOptions.Recursive"/> is <see langword="false"/> or unspecified and the entry is a non-empty directory, then this call fails;
    /// Else if it is <see langword="true"/> then it will recursively remove all sub-directories and entries in the directory.
    /// </param>
    public async Task RemoveEntryAsync(string name, FileSystemRemoveOptions? options = null)
    {
        await JSReference.InvokeVoidAsync("removeEntry", name, options);
    }

    /// <summary>
    /// If <paramref name="possibleDescendant"/> is equal to the same as this directory, path will be an empty array.<br />
    /// If <paramref name="possibleDescendant"/> is a direct child of this directory, path will be an array containing child’s name.<br />
    /// If <paramref name="possibleDescendant"/> is a descendant of this directory, path will be an array containing the names of all the intermediate directories and child’s name as last element.
    /// For example if directory represents <c>/home/user/project</c> and child represents <c>/home/user/project/foo/bar</c>, this will return <c>["foo", "bar"]</c>.<br />
    /// Otherwise (if this directory and <paramref name="possibleDescendant"/> are not related), path will be <see langword="null"/>.
    /// </summary>
    /// <param name="possibleDescendant">Some <see cref="IFileSystemHandle"/> that you want the path to from this <see cref="FileSystemDirectoryHandle"/>.</param>
    public async Task<string[]?> ResolveAsync(IFileSystemHandle possibleDescendant)
    {
        return await JSReference.InvokeAsync<string[]?>("resolve", possibleDescendant.JSReference);
    }
}
