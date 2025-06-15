namespace KristofferStrube.Blazor.FileSystem
{
    public interface IFileSystemHandleInProcess : IFileSystemHandle
    {
        FileSystemHandleKind Kind { get; }
        string Name { get; }
    }
}