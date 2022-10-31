namespace Animato.Messaging.Infrastructure.Services.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class InMemoryTemplateRepository : ITemplateRepository
{
    private readonly List<DocumentTemplate> templates;
    private readonly List<DocumentTemplateContent> templateContents;
    private readonly List<QueueTemplate> queueTemplates;
    private readonly ILogger<InMemoryTemplateRepository> logger;

    public InMemoryTemplateRepository(InMemoryDataContext dataContext, ILogger<InMemoryTemplateRepository> logger)
    {
        if (dataContext is null)
        {
            throw new ArgumentNullException(nameof(dataContext));
        }

        templates = dataContext.Templates;
        templateContents = dataContext.TemplateContents;
        queueTemplates = dataContext.QueueTemplates;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<IEnumerable<DocumentTemplate>> FindAll(CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(templates.AsEnumerable());
        }
        catch (Exception exception)
        {
            logger.TemplatesLoadingError(exception);
            throw;
        }
    }

    public async Task<DocumentTemplate> GetById(DocumentTemplateId templateId, CancellationToken cancellationToken)
    {
        var template = await FindById(templateId, cancellationToken);

        if (template is null)
        {
            throw new NotFoundException(nameof(DocumentTemplate), templateId);
        }

        return template;
    }

    public Task<DocumentTemplate> FindById(DocumentTemplateId templateId, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(templates.FirstOrDefault(u => u.Id == templateId));
        }
        catch (Exception exception)
        {
            logger.TemplatesLoadingError(exception);
            throw;
        }
    }

    public Task<DocumentTemplate> Create(DocumentTemplate documentTemplate, CancellationToken cancellationToken)
    {
        if (documentTemplate is null)
        {
            throw new ArgumentNullException(nameof(documentTemplate));
        }

        if (documentTemplate.Id == DocumentTemplateId.Empty)
        {
            documentTemplate.Id = DocumentTemplateId.New();
        }

        try
        {
            templates.Add(documentTemplate);
            return Task.FromResult(documentTemplate);
        }
        catch (Exception exception)
        {
            logger.TemplatesCreatingError(exception);
            throw;
        }
    }

    public Task<DocumentTemplate> Update(DocumentTemplate documentTemplate, CancellationToken cancellationToken)
    {
        if (documentTemplate is null)
        {
            throw new ArgumentNullException(nameof(documentTemplate));
        }

        try
        {
            var storedTemplate = templates.FirstOrDefault(a => a.Id == documentTemplate.Id);

            if (storedTemplate == null)
            {
                throw new NotFoundException(nameof(Application), documentTemplate.Id);
            }

            templates.Remove(storedTemplate);
            templates.Add(documentTemplate);

            return Task.FromResult(documentTemplate);
        }
        catch (Exception exception)
        {
            logger.TemplatesUpdatingError(exception);
            throw;
        }
    }

    public Task Delete(DocumentTemplateId templateId, CancellationToken cancellationToken)
    {
        try
        {
            queueTemplates.RemoveAll(q => q.TemplateId == templateId);
            templateContents.RemoveAll(c => c.Id == templateId);
            return Task.FromResult(templates.RemoveAll(a => a.Id == templateId));
        }
        catch (Exception exception)
        {
            logger.TemplatesDeletingError(exception);
            throw;
        }
    }

    public Task Clear(CancellationToken cancellationToken)
    {
        queueTemplates.Clear();
        templateContents.Clear();
        templates.Clear();
        return Task.CompletedTask;
    }

    public Task UpdateContent(DocumentTemplateId templateId, string fileName, Stream content, CancellationToken cancellationToken)
    {
        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        try
        {
            var storedTemplate = templateContents.FirstOrDefault(a => a.Id == templateId);

            if (storedTemplate is not null)
            {
                templateContents.Remove(storedTemplate);
            }

            content.Position = 0;
            var memoryStream = new MemoryStream();
            content.CopyTo(memoryStream);

            var templateContent = new DocumentTemplateContent()
            {
                Id = templateId,
                FileName = fileName,
                Content = memoryStream,
            };

            templateContents.Add(templateContent);

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.TemplatesUpdatingError(exception);
            throw;
        }
    }

    public Task<IEnumerable<DocumentTemplate>> FindByQueue(QueueId queueId, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(queueTemplates.Where(q => q.QueueId == queueId)
                .Join(templates, q => q.TemplateId, t => t.Id, (qt, t) => t));
        }
        catch (Exception exception)
        {
            logger.TemplatesLoadingError(exception);
            throw;
        }
    }

    public Task AddToQueue(DocumentTemplateId templateId, QueueId queueId, CancellationToken cancellationToken)
    {
        try
        {
            queueTemplates.Add(new QueueTemplate() { QueueId = queueId, TemplateId = templateId });
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.TemplatesUpdatingError(exception);
            throw;
        }
    }

    public Task RemoveFromQueue(DocumentTemplateId templateId, QueueId queueId, CancellationToken cancellationToken)
    {
        try
        {
            queueTemplates.RemoveAll(qt => qt.QueueId == queueId && qt.TemplateId == templateId);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.TemplatesUpdatingError(exception);
            throw;
        }
    }
}
