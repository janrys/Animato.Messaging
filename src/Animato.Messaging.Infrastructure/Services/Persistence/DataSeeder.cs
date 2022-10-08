namespace Animato.Messaging.Infrastructure.Services.Persistence;

using System;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Microsoft.Extensions.Logging;

public class DataSeeder : IDataSeeder
{
    private readonly GlobalOptions globalOptions;
    private readonly IQueueRepository queueRepository;
    private readonly ITemplateRepository templateRepository;

    private readonly ILogger<DataSeeder> logger;

    public DataSeeder(GlobalOptions globalOptions
        , IQueueRepository queueRepository
        , ITemplateRepository templateRepository
        , ILogger<DataSeeder> logger)
    {
        this.globalOptions = globalOptions ?? throw new ArgumentNullException(nameof(globalOptions));
        this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
        this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Seed()
    {
        if (!globalOptions.ClearAndSeedData)
        {
            return;
        }

        await Clear();
        await SeedQueues();
        await SeedTemplates();
    }

    private async Task Clear()
    {
        await queueRepository.Clear(CancellationToken.None);
        await templateRepository.Clear(CancellationToken.None);
    }

    private async Task SeedQueues()
    {
        //seededScopes.Add(await scopeRepository.Create(Scope.All, Scope.All.Id, CancellationToken.None));
        //seededScopes.Add(await scopeRepository.Create(Scope.General, Scope.General.Id, CancellationToken.None));
        //seededScopes.Add(await scopeRepository.Create(Scope.Online, Scope.Online.Id, CancellationToken.None));
        //seededScopes.Add(await scopeRepository.Create(Scope.Phone, Scope.Phone.Id, CancellationToken.None));
        //seededScopes.Add(await scopeRepository.Create(Scope.Role, Scope.Role.Id, CancellationToken.None));
        //seededScopes.Add(await scopeRepository.Create(Scope.Mail, Scope.Mail.Id, CancellationToken.None));


        //foreach (var application in seededApplications)
        //{
        //    await applicationRepository.CreateApplicationScopes(application.Id
        //        , CancellationToken.None
        //        , seededScopes.Where(s => s.Id != Scope.Phone.Id).Select(s => s.Id).ToArray());
        //}
        await Task.CompletedTask;
        logger.DataSeededInformation("Queues");
    }


    private async Task SeedTemplates()
    {
        await Task.CompletedTask;

        logger.DataSeededInformation("Templates");
    }
}
