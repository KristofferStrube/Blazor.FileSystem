using Microsoft.Extensions.DependencyInjection;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// Extensions for adding <see cref="IStorageManagerService"/> and <see cref="IStorageManagerServiceInProcess"/> to the service collection.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="IStorageManagerService"/> to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the service to.</param>
    public static IServiceCollection AddStorageManagerService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IStorageManagerService, StorageManagerService>();
    }

    /// <summary>
    /// Adds a <see cref="IStorageManagerServiceInProcess"/> to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the service to.</param>
    public static IServiceCollection AddStorageManagerServiceInProcess(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IStorageManagerServiceInProcess, StorageManagerServiceInProcess>()
            .AddScoped(sp =>
            {
                IStorageManagerServiceInProcess service = sp.GetRequiredService<IStorageManagerServiceInProcess>();
                return (IStorageManagerService)service;
            });
    }
}
