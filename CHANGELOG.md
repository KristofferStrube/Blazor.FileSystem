# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Removed
- Removed synchronous `FileSystemDirectoryHandle.Create`, `FileSystemFileHandle.Create`, `FileSystemHandle.Create`, and `FileSystemWritableFileStream.Create` creator methods in favor of asynchronous `CreateAsync` methods.
- Removed `FileSystemOptions` class and the overloaded methods that used it for customizing the helper-module path, as the same could be achieved with an `importmap`.
### Changed
- Changed the version of `Blazor.FileAPI` to use the newest version, which is 0.4.0.
- Changed constructors of `FileSystemHandle`, `FileSystemHandleInProcess`, `FileSystemFileHandle`, `FileSystemFileHandleInProcess`, `FileSystemDirectoryHandle`, `FileSystemDirectoryHandleInProcess`, and `FileSystemWritableFileStreamInProcess` to be protected instead of internal so that they can be extended.
- Changed all non-creator methods that return `IJSWrappers` so that their returned objects will dispose of their `JSReference` when they are disposed. As an example, `File`s returned from `FileSystemFileHandle.GetFileAsync` will now dispose their `JSReference` when the `File` is disposed, so that you don't need to manually dispose the `JSReference` before disposing the `File` itself.
### Added
- Added XML Documentation to all public types and members.
- Added target for .NET 8.
- Added `CreateAsync` creator methods for all wrapper classes that were missing them.
- Added `IAsyncDisposable` implementation to all wrapper classes that ensures that their helpers and `JSReference`s are disposed.
### Fixed
- Fixed that `FileSystemDirectoryHandleInProcess.ValuesAsync` would instantiate a helper module, which would not be disposed.

## [0.3.1] - 2023-10-19
### Fixed
- Fixed that writing `WriteParams` with a `Blob` used the wrong JS helper reference.

## [0.3.0] - 2023-03-16
### Changed
- Changed .NET version to `7.0`.
- Changed the version of Blazor.FileAPI to use the newest version which is `0.3.0`.
### Added
- Added the generation of a documentation file packaging all XML comments with the package.

## [0.2.0] - 2023-01-25
### Added
- Added interfaces `IFileSystemHandle` and `IFileSystemHandleInProcess`.
### Changed
- Changed `FileSystemHandle.IsSameEntryAsync` to take `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandle.ResolveAsync` to take `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandle.ValuesAsync` to return `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandleInProcess.ValuesAsync` to return `IFileSystemHandleInProcess` instead of `FileSystemHandleInProcess`.
