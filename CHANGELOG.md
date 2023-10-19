# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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