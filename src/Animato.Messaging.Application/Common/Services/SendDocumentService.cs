namespace Animato.Messaging.Application.Common.Services;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Documents;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class SendDocumentService : ISendDocumentService, IDisposable
{
    private readonly ConcurrentQueue<DocumentId> documents = new();
    private readonly IJobRepository jobRepository;
    private readonly IDocumentSenderFactory documentSenderFactory;
    private readonly ITemplateRepository templateRepository;
    private readonly IApplicationEventService applicationEventService;
    private readonly ITargetRepository targetRepository;
    private readonly IFileRepository fileRepository;
    private readonly ILogger<ProcessDocumentService> logger;
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private bool disposedValue;

    public SendDocumentService(IJobRepository jobRepository
        , IDocumentSenderFactory documentSenderFactory
        , ITemplateRepository templateRepository
        , IApplicationEventService applicationEventService
        , ITargetRepository targetRepository
        , IFileRepository fileRepository
        , ILogger<ProcessDocumentService> logger)
    {
        this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
        this.documentSenderFactory = documentSenderFactory ?? throw new ArgumentNullException(nameof(documentSenderFactory));
        this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
        this.applicationEventService = applicationEventService ?? throw new ArgumentNullException(nameof(applicationEventService));
        this.targetRepository = targetRepository ?? throw new ArgumentNullException(nameof(targetRepository));
        this.fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task CheckWaiting(CancellationToken cancellationToken)
    {
        var waitingDocuments = await jobRepository.GetDocumentsToSend(cancellationToken);

        if (waitingDocuments.Any())
        {
            waitingDocuments.ToList().ForEach(d => documents.Enqueue(d));
        }
    }

    public Task Enqueue(DocumentId documentId, CancellationToken cancellationToken)
    {
        documents.Enqueue(documentId);
        Task.Run(() => Send(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public async Task Send(CancellationToken cancellationToken)
    {
        var documentCount = semaphore.CurrentCount;
        logger.DocumentProcessingDebug(0);
        if (documentCount == 0)
        {
            return;
        }

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            do
            {
                if (documents.TryDequeue(out var documentId))
                {
                    await SendDocument(documentId, cancellationToken);
                }

                if (documents.IsEmpty)
                {
                    await CheckWaiting(cancellationToken);
                }

            } while (!documents.IsEmpty);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task SendDocument(DocumentId documentId, CancellationToken cancellationToken)
    {
        ProcessedDocument processedDocument = null;
        try
        {
            logger.StartSendingDocumentInformation(documentId);
            processedDocument = await jobRepository.StartSendingDocument(documentId, cancellationToken);

            if (processedDocument == null)
            {
                return;
            }

            var target = await targetRepository.GetById(processedDocument.TargetId, cancellationToken);
            var processor = documentSenderFactory.GetSender(processedDocument.TargetType, target);
            var file = await fileRepository.GetFile(processedDocument.FilePath, cancellationToken);


            foreach (var targetId in inputDocument.TargetIds)
            {
                var sendDocument = SendDocument.Create(inputDocument);
                sendDocument.Processed = DateTime.UtcNow;
                sendDocument.TargetId = targetId;
                await jobRepository.SendDocument(sendDocument, cancellationToken);
                await jobRepository.RemoveProcessingJob(jobId, cancellationToken);
                await applicationEventService.Publish(new DocumentProcessedEvent(sendDocument.Id), cancellationToken);
            }
            logger.FinishedProcessingDocumentInformation(jobId);

        }
        catch (Exception exception)
        {
            var failedDocument = new FailedDocument()
            {
                Document = inputDocument,
                Errors = new List<string>(),
                Id = inputDocument.Id,
                Failed = DateTime.UtcNow,
                DocumentPriority = inputDocument.DocumentPriority,
                JobId = jobId,
                QueueId = inputDocument.QueueId,
                QueuePriority = inputDocument.QueuePriority,
                Received = inputDocument.Received,
                ScheduleSendDate = inputDocument.ScheduleSendDate,
                TargetType = inputDocument.TargetType
            };
            failedDocument.Errors.Add($"Processing job failed {exception.GetType().Name} {exception.Message}");

            await jobRepository.FailDocument(failedDocument, cancellationToken);
            await jobRepository.RemoveProcessingJob(jobId, cancellationToken);
            logger.FinishedProcessingDocumentError(jobId, exception);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                semaphore.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ProcessDocumentService()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
