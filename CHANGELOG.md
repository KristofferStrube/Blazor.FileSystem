# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2023-01-25
### Added
- Added interfaces `IFileSystemHandle` and `IFileSystemHandleInProcess`.
### Changed
- Changed `FileSystemHandle.IsSameEntryAsync` to take `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandle.ResolveAsync` to take `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandle.ValuesAsync` to return `IFileSystemHandle` instead of `FileSystemHandle`.
- Changed `FileSystemDirectoryHandleInProcess.ValuesAsync` to return `IFileSystemHandleInProcess` instead of `FileSystemHandleInProcess`.