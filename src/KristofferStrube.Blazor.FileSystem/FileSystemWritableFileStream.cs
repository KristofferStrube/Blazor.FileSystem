﻿using KristofferStrube.Blazor.FileAPI;
using KristofferStrube.Blazor.FileSystem.Extensions;
using KristofferStrube.Blazor.Streams;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;
using File = KristofferStrube.Blazor.FileAPI.File;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// A <see cref="WritableStream"/> that can write to and seek in a <see cref="File"/>.
/// </summary>
/// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemwritablefilestream">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class FileSystemWritableFileStream : WritableStream, IJSCreatable<FileSystemWritableFileStream>
{
    /// <inheritdoc cref="BaseJSWrapper.helperTask"/>
    protected Lazy<Task<IJSObjectReference>> FileSystemHelperTask { get; }

    /// <inheritdoc/>
    public override bool CanSeek => true;

    /// <inheritdoc/>
    public override long Position { get; set; }

    /// <inheritdoc/>
    public static new async Task<FileSystemWritableFileStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new CreationOptions());
    }

    /// <inheritdoc/>
    public static new Task<FileSystemWritableFileStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new FileSystemWritableFileStream(jSRuntime, jSReference, FileSystemOptions.DefaultInstance, options));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemWritableFileStream"/> with options for where the JS helper module will be found at.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">Options for what path the JS helper module will be found at.</param>
    public static Task<FileSystemWritableFileStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
    {
        return Task.FromResult(new FileSystemWritableFileStream(jSRuntime, jSReference, options, new() { DisposesJSReference = true }));
    }

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <see cref="FileSystemWritableFileStream"/> with options for where the JS helper module will be found at and whether its JS reference should be disposed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="fileSystemOptions">Options for what path the JS helper module will be found at.</param>
    /// <param name="creationOptions">Options for what path the JS helper module will be found at.</param>
    public static Task<FileSystemWritableFileStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions creationOptions)
    {
        return Task.FromResult(new FileSystemWritableFileStream(jSRuntime, jSReference, fileSystemOptions, creationOptions));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference)"/>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static new FileSystemWritableFileStream Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new FileSystemWritableFileStream(jSRuntime, jSReference, FileSystemOptions.DefaultInstance, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileSystemWritableFileStream(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions fileSystemOptions, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        FileSystemHelperTask = new(() => jSRuntime.GetHelperAsync(fileSystemOptions));
    }

    /// <inheritdoc/>
    public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await JSReference.InvokeVoidAsync("write", buffer.ToArray());
    }

    /// <summary>
    /// Writes the content of data into the file associated with stream at the current file cursor offset.
    /// No changes are written to the actual file on disk until the stream has been closed.
    /// Changes are typically written to a temporary file instead.
    /// </summary>
    /// <param name="data">The data that will be written to the stream.</param>
    public async Task WriteAsync(string data)
    {
        await JSReference.InvokeVoidAsync("write", data);
    }

    /// <inheritdoc cref="WriteAsync(string)"/>
    public async Task WriteAsync(byte[] data)
    {
        await JSReference.InvokeVoidAsync("write", data);
    }

    /// <inheritdoc cref="WriteAsync(string)"/>
    public async Task WriteAsync(Blob data)
    {
        await JSReference.InvokeVoidAsync("write", data.JSReference);
    }

    /// <summary>
    /// <list type="table">
    /// <item>
    /// If <see cref="BaseWriteParams.Type"/> of <paramref name="data"/> is set to <see cref="WriteCommandType.Write"/> it will write the data into the file associated with the stream.
    /// If the <see cref="BaseWriteParams.Position"/> is set to some value then it will write the content at this position;
    /// Else it will write the content at the current file cursor offset.<br />
    /// No changes are written to the actual file on disk until the stream has been closed.<br />
    /// Changes are typically written to a temporary file instead.
    /// </item>
    /// <item>
    /// If <see cref="BaseWriteParams.Type"/> of <paramref name="data"/> is set to <see cref="WriteCommandType.Seek"/>
    /// it will move the file cursor offset to the position specified in <paramref name="data"/>'s <see cref="BaseWriteParams.Position"/> property.
    /// </item>
    /// <item>
    /// If <see cref="BaseWriteParams.Type"/> of <paramref name="data"/> is set to <see cref="WriteCommandType.Truncate"/> it will resize the file associated with the stream to be the size specified in <paramref name="data"/>'s <see cref="BaseWriteParams.Size"/> property.
    /// If <see cref="BaseWriteParams.Size"/> is larger than the current file size this pads the file with null bytes, otherwise it truncates the file.
    /// The file cursor is updated when truncate is called.
    /// If the cursor is smaller than <see cref="BaseWriteParams.Size"/>, it remains unchanged.
    /// If the cursor is larger than <see cref="BaseWriteParams.Size"/>, it is set to <see cref="BaseWriteParams.Size"/> to ensure that subsequent writes do not error.
    /// No changes are written to the actual file until on disk until the stream has been closed.
    /// Changes are typically written to a temporary file instead.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="data">The argument for how to process by performing one of the actions: <see cref="WriteCommandType.Write"/>, <see cref="WriteCommandType.Seek"/>, <see cref="WriteCommandType.Truncate"/>.</param>
    public async Task WriteAsync(BlobWriteParams data)
    {
        await JSReference.InvokeVoidAsync("write", data);
    }

    /// <inheritdoc cref="WriteAsync(BlobWriteParams)"/>
    public async Task WriteAsync(StringWriteParams data)
    {
        await JSReference.InvokeVoidAsync("write", data);
    }

    /// <inheritdoc cref="WriteAsync(BlobWriteParams)"/>
    public async Task WriteAsync(ByteArrayWriteParams data)
    {
        await JSReference.InvokeVoidAsync("write", data);
    }

    /// <summary>
    /// Updates the current file cursor offset the position bytes from the top of the file.
    /// </summary>
    /// <param name="position">The new cursor position</param>
    public async Task SeekAsync(ulong position)
    {
        Position = (long)position;
        await JSReference.InvokeVoidAsync("seek", position);
    }

    /// <summary>
    /// Resizes the file associated with stream to be <paramref name="size"/> bytes long. If  <paramref name="size"/> is larger than the current file size this pads the file with null bytes, otherwise it truncates the file.
    /// The file cursor is updated when truncate is called.
    /// If the cursor is smaller than  <paramref name="size"/>, it remains unchanged.
    /// If the cursor is larger than  <paramref name="size"/>, it is set to  <paramref name="size"/> to ensure that subsequent writes do not error.
    /// No changes are written to the actual file until on disk until the stream has been closed. Changes are typically written to a temporary file instead.
    /// </summary>
    /// <param name="size">The new size</param>
    public async Task TruncateAsync(ulong size)
    {
        await JSReference.InvokeVoidAsync("truncate", size);
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        if (FileSystemHelperTask.IsValueCreated)
        {
            IJSObjectReference module = await FileSystemHelperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
