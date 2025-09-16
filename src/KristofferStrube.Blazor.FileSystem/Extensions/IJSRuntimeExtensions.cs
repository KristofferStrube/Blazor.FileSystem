using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem.Extensions;

internal static class IJSRuntimeExtensions
{
    internal static async Task<IJSObjectReference> GetHelperAsync(this IJSRuntime jSRuntime, FileSystemOptions options)
    {
        return await GetHelperAsync<IJSObjectReference>(jSRuntime, options);
    }

    internal static async Task<IJSInProcessObjectReference> GetInProcessHelperAsync(this IJSRuntime jSRuntime, FileSystemOptions options)
    {
        return await GetHelperAsync<IJSInProcessObjectReference>(jSRuntime, options);
    }

    private static async Task<T> GetHelperAsync<T>(IJSRuntime jSRuntime, FileSystemOptions options)
    {
        return await jSRuntime.InvokeAsync<T>("import", options.FullScriptPath);
    }
}
