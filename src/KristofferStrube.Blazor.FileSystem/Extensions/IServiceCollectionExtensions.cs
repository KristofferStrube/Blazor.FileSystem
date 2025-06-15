using Microsoft.Extensions.DependencyInjection;

namespace KristofferStrube.Blazor.FileSystem
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageManagerService(this IServiceCollection serviceCollection)
        {
            return AddStorageManagerService(serviceCollection, null);
        }

        public static IServiceCollection AddStorageManagerService(this IServiceCollection serviceCollection, Action<FileSystemOptions>? configure)
        {
            ConfigureFsOptions(serviceCollection, configure);

            return serviceCollection.AddScoped<IStorageManagerService, StorageManagerService>();
        }

        public static IServiceCollection AddStorageManagerServiceInProcess(this IServiceCollection serviceCollection)
        {
            return AddStorageManagerServiceInProcess(serviceCollection, null);
        }

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

            services.Configure(configure);
            configure(FileSystemOptions.DefaultInstance);
        }

    }
}
