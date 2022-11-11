namespace Animato.Messaging.Infrastructure.Services.Persistence;

using System;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using Animato.Messaging.Infrastructure.Services.DocumentProcessing;
using Microsoft.Extensions.Logging;

public class DataSeeder : IDataSeeder
{
    private readonly GlobalOptions globalOptions;
    private readonly IQueueRepository queueRepository;
    private readonly ITemplateRepository templateRepository;
    private readonly List<Queue> seededQueues = new();
    private readonly List<DocumentTemplate> seededTemplates = new();

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
        seededQueues.Add(new Queue()
        {
            Id = new QueueId(),
            IsActive = true,
            Name = "default queue",
            Priority = 10,
        });

        foreach (var queue in seededQueues)
        {
            await queueRepository.Create(queue, CancellationToken.None);
        }

        logger.DataSeededInformation("Queues");
    }


    private async Task SeedTemplates()
    {
        var testTemplate = new DocumentTemplate()
        {
            Id = new DocumentTemplateId(),
            Name = "test template",
            TargetType = TargetType.Email,
            ProcessorId = new ProcessorId(Guid.Parse(DebugTemplateProcessor.ProcessorId)),
        };

        seededTemplates.Add(testTemplate);

        foreach (var template in seededTemplates)
        {
            await templateRepository.Create(template, CancellationToken.None);
            await templateRepository.AddToQueue(template.Id, seededQueues.First().Id, CancellationToken.None);
        }

        var myTemplate = "my template content";
        using var templateStream = GetStreamFromString(myTemplate);
        await templateRepository.UpdateContent(testTemplate.Id, "testtemplate.html", templateStream, CancellationToken.None);

        logger.DataSeededInformation("Templates");
    }

    private static Stream GetStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
