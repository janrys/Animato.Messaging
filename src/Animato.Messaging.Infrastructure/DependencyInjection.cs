namespace Animato.Messaging.Infrastructure;

using System.Reflection;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Infrastructure.Services;
using Animato.Messaging.Infrastructure.Services.Messaging;
using Animato.Messaging.Infrastructure.Services.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var globalOptions = new GlobalOptions();
        configuration.Bind(GlobalOptions.ConfigurationKey, globalOptions);

        if (globalOptions.Persistence.Equals("inmemory", StringComparison.OrdinalIgnoreCase))
        {
            services.AddInMemoryPersistence();
        }
        services.AddSingleton<IDataSeeder, DataSeeder>();

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IDomainEventService>(service
            => new LoggingDomainEventService(new NullDomainEventService(), service.GetService<ILogger<LoggingDomainEventService>>()));

        return services;
    }

    private static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryDataContext>();
        services.AddSingleton<IQueueRepository, InMemoryQueueRepository>();
        services.AddSingleton<ITemplateRepository, InMemoryTemplateRepository>();
        return services;
    }
}
