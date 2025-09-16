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
        return await CreateAsync(jSRuntime, jSReference, new CreationOptions());
    }

    /// <inheritdoc/>
    public static Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new FileSystemHandle(jSRuntime, jSReference, FileSystemOptions.DefaultInstance, options));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemHandle"/> with options for where the JS helper module will be found at.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">Options for what path the JS helper module will be found at.</param>
    public static Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return Task.FromResult(new FileSystemHandle(jSRuntime, jSReference, options, new() { DisposesJSReference = true }));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemHandle"/> with options for where the JS helper module will be found at and whether its JS reference should be disposed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="fileSystemOptions">Options for what path the JS helper module will be found at.</param>
    /// <param name="creationOptions">Options for what path the JS helper module will be found at.</param>
    public static Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions creationOptions)
    {
        return Task.FromResult(new FileSystemHandle(jSRuntime, jSReference, fileSystemOptions, creationOptions));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)" path="/summary"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static FileSystemHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Create(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)" path="/summary"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static FileSystemHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return new(jSRuntime, jSReference, options, new());
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileSystemHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, fileSystemOptions, options) { }

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
