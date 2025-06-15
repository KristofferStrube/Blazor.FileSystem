using KristofferStrube.Blazor.FileSystem.Extensions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem
{
    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#filesystemdirectoryhandle">FileSystemDirectoryHandle browser specs</see>
    /// </summary>
    public class FileSystemDirectoryHandleInProcess : FileSystemDirectoryHandle, IFileSystemHandleInProcess
    {
        public new IJSInProcessObjectReference JSReference;
        protected readonly IJSInProcessObjectReference inProcessHelper;

        public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
        }

        public static async Task<FileSystemDirectoryHandleInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, FileSystemOptions options)
        {
            IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync(options);
            return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSReference, options);
        }

        internal FileSystemDirectoryHandleInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, FileSystemOptions options) : base(jSRuntime, jSReference, options)
        {
            this.inProcessHelper = inProcessHelper;
            JSReference = jSReference;
        }

        public FileSystemHandleKind Kind => inProcessHelper.Invoke<FileSystemHandleKind>("getAttribute", JSReference, "kind");

        public string Name => inProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

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
                            var fileSystemHandle = await FileSystemHandleInProcess.CreateAsync(
                                jSRuntime,
                                await jSEntries.InvokeAsync<IJSInProcessObjectReference>("at", i),
                                options);
                            return (fileSystemHandle.Kind is FileSystemHandleKind.File)
                                ? await FileSystemFileHandleInProcess.CreateAsync(jSRuntime, fileSystemHandle.JSReference, options)
                                : await FileSystemDirectoryHandleInProcess.CreateAsync(jSRuntime, fileSystemHandle.JSReference, options);
                        }
                    )
                    .ToArray()
            );
        }

        public new async Task<FileSystemFileHandleInProcess> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
        {
            IJSInProcessObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getFileHandle", name, options);
            return new FileSystemFileHandleInProcess(jSRuntime, inProcessHelper, jSFileSystemFileHandle, this.options);
        }

        public new async Task<FileSystemDirectoryHandleInProcess> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
        {
            IJSInProcessObjectReference jSFileSystemDirectoryHandle = await JSReference.InvokeAsync<IJSInProcessObjectReference>("getDirectoryHandle", name, options);
            return new FileSystemDirectoryHandleInProcess(jSRuntime, inProcessHelper, jSFileSystemDirectoryHandle, this.options);
        }
    }
}
