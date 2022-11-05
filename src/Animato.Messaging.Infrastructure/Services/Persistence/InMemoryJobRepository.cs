namespace Animato.Messaging.Infrastructure.Services.Persistence;

using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using Microsoft.Extensions.Logging;

public class InMemoryJobRepository : IJobRepository
{
    private readonly ILogger<InMemoryJobRepository> logger;
    private readonly List<InputDocument> receivedDocuments;
    private readonly List<ProcessedDocument> processedDocuments;
    private readonly List<SendDocument> sendDocuments;
    private readonly List<FailedDocument> failedDocuments;

    public InMemoryJobRepository(InMemoryDataContext dataContext, ILogger<InMemoryJobRepository> logger)
    {
        if (dataContext is null)
        {
            throw new ArgumentNullException(nameof(dataContext));
        }

        receivedDocuments = dataContext.ReceivedDocuments;
        processedDocuments = dataContext.ProcessedDocuments;
        sendDocuments = dataContext.SendDocuments;
        failedDocuments = dataContext.FailedDocuments;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<JobStatusDto> GetStatus(JobId jobId, CancellationToken cancellationToken)
    {
        var status = new JobStatusDto() { Id = jobId.ToString() };

        var documents = new List<BaseDocument>();

        documents.AddRange(receivedDocuments.Where(d => d.JobId == jobId));
        documents.AddRange(processedDocuments.Where(d => d.JobId == jobId));
        documents.AddRange(sendDocuments.Where(d => d.JobId == jobId));
        documents.AddRange(failedDocuments.Where(d => d.JobId == jobId));

        status.Documents.AddRange(documents.Select(d => new DocumentStatusDto() { Id = d.Id.ToString(), Status = GetStatusForDocument(d) }));

        return Task.FromResult(status);
    }

    public Task<DocumentStatusDto> GetStatus(DocumentId documentId, CancellationToken cancellationToken)
    {
        try
        {
            var status = new DocumentStatusDto() { Id = documentId.ToString() };
            var document = (BaseDocument)receivedDocuments.FirstOrDefault(d => d.Id == documentId)
                ?? (BaseDocument)processedDocuments.FirstOrDefault(d => d.Id == documentId)
                ?? (BaseDocument)sendDocuments.FirstOrDefault(d => d.Id == documentId)
                ?? failedDocuments.FirstOrDefault(d => d.Id == documentId);

            if (document is null)
            {
                throw new NotFoundException(nameof(Document), documentId);
            }

            status.Status = GetStatusForDocument(document);

            return Task.FromResult(status);
        }
        catch (Exception exception)
        {
            logger.DocumentsLoadingError(exception);
            throw;
        }
    }

    private DocumentStatus GetStatusForDocument(BaseDocument document)
    {
        switch (document)
        {
            case InputDocument inputDocument:
                return inputDocument.ProcessingStarted.HasValue ? DocumentStatus.BeingProcessed : DocumentStatus.WaitingToProcess;
            case FailedDocument:
                return DocumentStatus.Failed;
            case Domain.Entities.SendDocument:
                return DocumentStatus.Send;
            case ProcessedDocument processedDocument:
                if (processedDocument.SendingStarted.HasValue)
                {
                    return DocumentStatus.BeingSend;
                }
                else if (processedDocument.ScheduleSendDate >= DateTime.UtcNow)
                {
                    return DocumentStatus.WaitingForSchedule;
                }
                else
                {
                    return DocumentStatus.WaitingToSend;
                }
            default:
                throw new ValidationException(ValidationException.CreateFailure(nameof(document), document.GetType().Name));
        }
    }

    public Task ProcessDocument(ProcessedDocument document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        try
        {
            processedDocuments.Add(document);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.DocumentsCreatingError(exception);
            throw;
        }
    }

    public Task ReceiveDocument(InputDocument document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (document.Id == DocumentId.Empty)
        {
            document.Id = DocumentId.New();
        }

        try
        {
            receivedDocuments.Add(document);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.DocumentsCreatingError(exception);
            throw;
        }
    }

    public Task SendDocument(SendDocument document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        try
        {
            sendDocuments.Add(document);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.DocumentsCreatingError(exception);
            throw;
        }
    }
    public Task FailDocument(FailedDocument document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        try
        {
            failedDocuments.Add(document);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.DocumentsCreatingError(exception);
            throw;
        }
    }
}
