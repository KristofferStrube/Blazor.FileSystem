using Microsoft.AspNetCore.Components;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Shared;


    // ElementExplorer.razor.cs

    public partial class ElementExplorer
    {
        private List<IFileSystemHandleInProcess> children = new();
        [Parameter, EditorRequired]
        public IFileSystemHandleInProcess Element { get; set; } = default !;
        [Parameter, EditorRequired]
        public Func<FileSystemFileHandle, Task> EditAction { get; set; } = default !;
        protected override async Task OnParametersSetAsync()
        {
            if (Element is not FileSystemDirectoryHandleInProcess directoryHandle)
                return;
            children = (await directoryHandle.ValuesAsync()).ToList();
        }

        async Task Remove(IFileSystemHandleInProcess element)
        {
            if (Element is not FileSystemDirectoryHandleInProcess { } directoryHandle
                || (await directoryHandle.ResolveAsync(element))!.Length is 0) return;
            await directoryHandle.RemoveEntryAsync(element.Name, new() { Recursive = true });
            children.Remove(element);
        }

        async Task AddFile()
        {
            if (Element is not FileSystemDirectoryHandleInProcess directoryHandle)
                return;
            children.Add(await directoryHandle.GetFileHandleAsync($"{Guid.NewGuid().ToString()[..4]}.txt", new() { Create = true }));
        }

        async Task AddDirectory()
        {
            if (Element is not FileSystemDirectoryHandleInProcess directoryHandle)
                return;
            children.Add(await directoryHandle.GetDirectoryHandleAsync(Guid.NewGuid().ToString()[..4], new() { Create = true }));
        }
    }