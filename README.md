[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/issues)
[![GitHub forks](https://img.shields.io/github/forks/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/network/members)
[![GitHub stars](https://img.shields.io/github/stars/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/stargazers)

<!-- [![NuGet Downloads (official NuGet)](https://img.shields.io/nuget/dt/KristofferStrube.Blazor.FileSystem?label=NuGet%20Downloads)](https://www.nuget.org/packages/KristofferStrube.Blazor.FileSystem/)  -->

# Introduction
A Blazor wrapper for the [File System](https://fs.spec.whatwg.org/) browser API.

The API standardizes ways to handle files and directories. It also enables access to the **origin private file system** which is a virtual sandboxed file system. This project implements a wrapper around the API for Blazor so that we can easily and safely interact with it from Blazor.

# Demo
The sample project can be demoed at https://kristofferstrube.github.io/Blazor.FileSystem/

On each page you can find the corresponding code for the example in the top right corner.

On the [Status page](https://kristofferstrube.github.io/Blazor.FileSystem/Status) you can see how much of the WebIDL specs this wrapper has covered.

# Getting Started
## Prerequisites
You need to install .NET 6.0 or newer to use the library.

[Download .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

## Installation
You can install the package via Nuget with the Package Manager in your IDE or alternatively using the command line:
```bash
dotnet add package KristofferStrube.Blazor.FileSystem
```

# Usage
The package can be used in Blazor WebAssembly and Blazor Server projects. (Note that streaming of big files is not supported in Blazor Server due to bandwidth problems.)
## Import
You need to reference the package in order to use it in your pages. This can be done in `_Import.razor` by adding the following.
```razor
@using KristofferStrube.Blazor.FileSystem
```
## Add to service collection
The library has one service which is the `IStorageManagerService` which can be used to get access to the **origin private file system**. An easy way to make the service available in all your pages is by registering it in the `IServiceCollection` so that it can be dependency injected in the pages that need it. This is done in `Program.cs` by adding the following before you build the host and run it.
```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Other services are added.

builder.Services.AddStorageManagerService();

await builder.Build().RunAsync();
```
## Inject in page
Then the service can be injected in a page like so:
```razor
@inject IStorageManagerService StorageManagerService;
```
Then you can use `IStorageManagerService` to get a directory handle for the **origin private file system** and read/create a file in it like this:
```razor
<button @onclick="OpenAndReadFile">Open Single File and Read</button>

@code {
    private async Task OpenAndReadFile()
    {
        FileSystemDirectoryHandle directoryHandle = await StorageManagerService.GetOriginPrivateDirectoryAsync();
        FileSystemFileHandle fileHandle = await directoryHandle.GetFileHandleAsync("file.txt", new() { Create = true });
        FileAPI.File file = await fileHandle.GetFileAsync();
        // Do something with the file.
    }
}
```

The counterpart to getting a `File` via the `GetFileAsync()` method is getting a `FileSystemWritableFileStream` via the `CreateWritableAsync()` which can be used to write to the file referenced with the `FileSystemFileHandle` like this:
```csharp
FileSystemFileHandle fileHandle; // Some file handle
FileSystemWritableFileStream writable = await fileHandle.CreateWritableAsync();
await writable.WriteAsync("some text");
await writable.CloseAsync();
```
You need to close the `FileSystemWritableFileStream` to commit the written text. You can either do so explicitly as seen above or you can use it in a using statement like this:
```csharp
FileSystemFileHandle fileHandle; // Some file handle
await using FileSystemWritableFileStream writable = await fileHandle.CreateWritableAsync();
await writable.WriteAsync(new byte[] { 0, 1, 2, 3, 4, 5 });
```
This will automatically await the close when you get to the end of the current scope.

# Issues
Feel free to open issues on the repository if you find any errors with the package or have wishes for features.

# Related repositories
This project uses the *Blazor.FileAPI* package to return a rich `File` from the `GetFileAsync` method on a `FileSystemFileHandle` and when writing a `Blob` to a `FileSystemWritableFileSystem`.
- https://github.com/KristofferStrube/Blazor.FileAPI

This project is used in the *Blazor.FileSystemAccess* package as a basis for the file handles that it uses in its methods and extends on.
- https://github.com/KristofferStrube/Blazor.FileSystemAccess

# Related articles
This repository was build with inspiration and help from the following series of articles:

- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)
