[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/issues)
[![GitHub forks](https://img.shields.io/github/forks/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/network/members)
[![GitHub stars](https://img.shields.io/github/stars/KristofferStrube/Blazor.FileSystem)](https://github.com/KristofferStrube/Blazor.FileSystem/stargazers)

<!-- [![NuGet Downloads (official NuGet)](https://img.shields.io/nuget/dt/KristofferStrube.Blazor.FileSystem?label=NuGet%20Downloads)](https://www.nuget.org/packages/KristofferStrube.Blazor.FileSystem/)  -->

# Introduction
A Blazor wrapper for the [File System](https://fs.spec.whatwg.org/) browser API.

The API standardizes ways to handle files and directories. It also enables access to the **origin private file system** which is a virtual sandboxed file system. This project implements a wrapper around the API for Blazor so that we can easily and safely interact with it from Blazor.

_Disclaimer: This wrapper is still being developed so API coverage is limited._

# Related repositories
Wrapper for most of the types defined in the File System API are currently wrapped in the following repository but will be moved to this repository.
- https://github.com/KristofferStrube/Blazor.FileSystemAccess

# Related articles
This repository was build with inspiration and help from the following series of articles:

- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)
