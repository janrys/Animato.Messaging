using Animato.Messaging.WebApi.Extensions;
using Animato.Messaging.WebApi.Middlewares;
using Animato.Messaging.Application;
using Animato.Messaging.Infrastructure;
using Animato.Messaging.Infrastructure.AzureStorage;
using Serilog;

Log.Logger = Animato.Messaging.WebApi.Extensions.ServiceCollectionExtensions.CreateBootstrapLogger();
var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName();
Log.Information("Starting up {Application} {Version}", assemblyName.Name, assemblyName.Version);

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddCustomConfiguration(builder.Environment.EnvironmentName);
    builder.AddCustomLogging(builder.Configuration);
    builder.Services.AddApplication(builder.Configuration);
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddAzureInfrastructure(builder.Configuration);
    builder.Services.AddWebApi(builder.Environment);

    var app = builder.Build();
    //if (app.Environment.IsDevelopment())
    if (true)
    {
        app.UseCustomSwagger();
        app.UseDeveloperExceptionPage();
    }

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseCustomLogging(app.Configuration);
    app.UseCustomProblemDetails();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    var cookiePolicyOptions = new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Lax,
    };
    app.UseCookiePolicy(cookiePolicyOptions);
    app.MapControllers();
    app.UseCustomHealthChecks();

    app.Run();
    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Information("Shut down complete {Application} {Version}", assemblyName.Name, assemblyName.Version);
    Log.CloseAndFlush();
}


