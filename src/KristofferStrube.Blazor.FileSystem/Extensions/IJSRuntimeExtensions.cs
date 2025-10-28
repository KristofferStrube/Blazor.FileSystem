using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileSystem.Extensions;

internal static class IJSRuntimeExtensions
{
    private const string helperPath = "./_content/KristofferStrube.Blazor.FileSystem/KristofferStrube.Blazor.FileSystem.js";

    internal static async Task<IJSObjectReference> GetHelperAsync(this IJSRuntime jSRuntime)
    {
        return await jSRuntime.InvokeAsync<IJSObjectReference>("import", helperPath);
    }

    internal static async Task<IJSInProcessObjectReference> GetInProcessHelperAsync(this IJSRuntime jSRuntime)
    {
        return await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", helperPath);
    }
}
