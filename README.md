[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/issues)
[![GitHub forks](https://img.shields.io/github/forks/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/network/members)
[![GitHub stars](https://img.shields.io/github/stars/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/stargazers)

<!-- [![NuGet Downloads (official NuGet)](https://img.shields.io/nuget/dt/KristofferStrube.Blazor.FileSystem?label=NuGet%20Downloads)](https://www.nuget.org/packages/KristofferStrube.Blazor.FileSystem/)  -->

# Introduction
A Blazor wrapper for the [File System](https://fs.spec.whatwg.org/) browser API.

The API standardizes ways to handle files and directories. It also enables access to the **origin private file system** which is a virtual sandboxed file system. This project implements a wrapper around the API for Blazor so that we can easily and safely interact with it from Blazor.

## Demo
The sample project can be demoed at https://kristofferstrube.github.io/Blazor.FileSystem/

On each page you can find the corresponding code for the example in the top right corner.

On the [Status page](https://kristofferstrube.github.io/Blazor.FileSystem/Status) you can see how much of the WebIDL specs this wrapper has covered.

# Related repositories
The [Blazor.FileSystemAccess](https://github.com/KristofferStrube/Blazor.FileSystemAccess) library currently also have implementations for the types defined in this library, but will eventually change to use this library as a dependency.

# Related articles
This repository was build with inspiration and help from the following series of articles:

- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)
