namespace Animato.Messaging.Infrastructure.AzureStorage;

using System.Reflection;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Infrastructure.AzureStorage.Services;
using Animato.Messaging.Infrastructure.AzureStorage.Services.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAzureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var globalOptions = new GlobalOptions();
        configuration.Bind(GlobalOptions.ConfigurationKey, globalOptions);

        if (globalOptions.Persistence.Equals("azuretable", StringComparison.OrdinalIgnoreCase))
        {
            services.AddAzureTablePersistence(configuration);
        }

        return services;
    }

    private static IServiceCollection AddAzureTablePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var azureTableOptions = new AzureTableStorageOptions();
        configuration.Bind(AzureTableStorageOptions.ConfigurationKey, azureTableOptions);
        services.AddSingleton(azureTableOptions);

        services.AddSingleton<AzureTableStorageDataContext>();
        services.AddSingleton<IQueueRepository, AzureTableQueueRepository>();

        services.AddHealthChecks().AddCheck<TableStorageHealthCheck>(TableStorageHealthCheck.Name);
        return services;
    }
}
