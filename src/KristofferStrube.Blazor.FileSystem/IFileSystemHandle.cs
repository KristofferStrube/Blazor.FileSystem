using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem
{
    public interface IFileSystemHandle
    {
        public IJSObjectReference JSReference { get; }
        Task<FileSystemHandleKind> GetKindAsync();
        Task<string> GetNameAsync();
        Task<bool> IsSameEntryAsync(IFileSystemHandle other);
    }
}