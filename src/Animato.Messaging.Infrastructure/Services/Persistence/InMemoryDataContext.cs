namespace Animato.Messaging.Infrastructure.Services.Persistence;

using System.Collections.Generic;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class InMemoryDataContext
{
    private readonly ILogger<InMemoryDataContext> logger;

    public InMemoryDataContext(ILogger<InMemoryDataContext> logger)
    {
        this.logger = logger;
        this.logger.PersistenceLayerLoadingInformation("in-memory");
    }

    public List<Queue> Queues { get; set; } = new List<Queue>();
    public List<DocumentTemplate> Templates { get; set; } = new List<DocumentTemplate> { };
    public List<DocumentTemplateContent> TemplateContents { get; set; } = new List<DocumentTemplateContent> { };
}
