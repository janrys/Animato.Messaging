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
    public List<QueueTemplate> QueueTemplates { get; set; } = new List<QueueTemplate> { };
    public List<Target> Targets { get; set; } = new List<Target> { };
    public List<InputDocument> ReceivedDocuments { get; set; } = new List<InputDocument> { };
    public List<ProcessedDocument> ProcessedDocuments { get; set; } = new List<ProcessedDocument> { };
    public List<SendDocument> SendDocuments { get; set; } = new List<SendDocument> { };
    public List<FailedDocument> FailedDocuments { get; set; } = new List<FailedDocument> { };
    public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string> { };
}
