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
        return AddStorageManagerService(serviceCollection, null);
    }

    /// <summary>
    /// Adds a <see cref="IStorageManagerService"/> to the service collection with the option to configure where the helper JS module is located.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the service to.</param>
    /// <param name="configure">An action to configure the <see cref="FileSystemOptions"/> that defines where the helper JS module is located.</param>
    public static IServiceCollection AddStorageManagerService(this IServiceCollection serviceCollection, Action<FileSystemOptions>? configure)
    {
        ConfigureFsOptions(serviceCollection, configure);

        return serviceCollection.AddScoped<IStorageManagerService, StorageManagerService>();
    }

    /// <summary>
    /// Adds a <see cref="IStorageManagerServiceInProcess"/> to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the service to.</param>
    public static IServiceCollection AddStorageManagerServiceInProcess(this IServiceCollection serviceCollection)
    {
        return AddStorageManagerServiceInProcess(serviceCollection, null);
    }

    /// <summary>
    /// Adds a <see cref="IStorageManagerService"/> to the service collection with the option to configure where the helper JS module is located.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the service to.</param>
    /// <param name="configure">An action to configure the <see cref="FileSystemOptions"/> that defines where the helper JS module is located.</param>
    public static IServiceCollection AddStorageManagerServiceInProcess(this IServiceCollection serviceCollection, Action<FileSystemOptions>? configure)
    {
        ConfigureFsOptions(serviceCollection, configure);

        return serviceCollection
            .AddScoped<IStorageManagerServiceInProcess, StorageManagerServiceInProcess>()
            .AddScoped(sp =>
            {
                IStorageManagerServiceInProcess service = sp.GetRequiredService<IStorageManagerServiceInProcess>();
                return (IStorageManagerService)service;
            });
    }

    private static void ConfigureFsOptions(IServiceCollection services, Action<FileSystemOptions>? configure)
    {
        if (configure is null) { return; }

        _ = services.Configure(configure);
        configure(FileSystemOptions.DefaultInstance);
    }
}
