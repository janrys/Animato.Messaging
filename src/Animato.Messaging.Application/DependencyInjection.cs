namespace Animato.Messaging.Application;
using System.Reflection;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Behaviours;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Services;
using Animato.Messaging.Application.Security;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var globalOptions = new GlobalOptions();
        configuration.Bind(GlobalOptions.ConfigurationKey, globalOptions);
        services.AddSingleton(globalOptions);

        var oidcOptions = new OidcOptions();
        configuration.Bind(OidcOptions.ConfigurationKey, oidcOptions);
        services.AddSingleton(oidcOptions);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddSingleton<IAuthorizationService, StaticMapAuthorizationService>();
        services.AddSingleton<IApplicationEventService, ApplicationEventService>();
        services.AddSingleton<IProcessDocumentService, ProcessDocumentService>();
        services.AddSingleton<ISendDocumentService, SendDocumentService>();
        return services;
    }
}
