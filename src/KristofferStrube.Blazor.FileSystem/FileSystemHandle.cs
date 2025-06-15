using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem
{
    /// <summary>
    /// <see href="https://fs.spec.whatwg.org/#filesystemhandle">FileSystemHandle browser specs</see>
    /// </summary>
    public class FileSystemHandle : BaseJSWrapper, IFileSystemHandle
    {
        public static FileSystemHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return Create(jSRuntime, jSReference, FileSystemOptions.DefaultInstance);
        }

        public static FileSystemHandle Create(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options)
        {
            return new(jSRuntime, jSReference, options);
        }

        internal FileSystemHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, FileSystemOptions options) : base(jSRuntime, jSReference, options) { }

        public async Task<FileSystemHandleKind> GetKindAsync()
        {
            IJSObjectReference helper = await helperTask.Value;
            return await helper.InvokeAsync<FileSystemHandleKind>("getAttribute", JSReference, "kind");
        }

        public async Task<string> GetNameAsync()
        {
            IJSObjectReference helper = await helperTask.Value;
            return await helper.InvokeAsync<string>("getAttribute", JSReference, "name");
        }

        public async Task<bool> IsSameEntryAsync(IFileSystemHandle other)
        {
            return await JSReference.InvokeAsync<bool>("isSameEntry", other.JSReference);
        }
    }
}
