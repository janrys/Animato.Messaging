namespace Animato.Messaging.Application.Common.Services;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class ProcessDocumentService : IProcessDocumentService, IDisposable
{
    private readonly ConcurrentQueue<JobId> jobs = new();
    private readonly IJobRepository jobRepository;
    private readonly ITemplateProcessorFactory templateProcessorFactory;
    private readonly ITemplateRepository templateRepository;
    private readonly ILogger<ProcessDocumentService> logger;
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private bool disposedValue;

    public ProcessDocumentService(IJobRepository jobRepository
        , ITemplateProcessorFactory templateProcessorFactory
        , ITemplateRepository templateRepository
        , ILogger<ProcessDocumentService> logger)
    {
        this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
        this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
        this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task CheckWaiting(CancellationToken cancellationToken)
    {
        var waitingJobs = await jobRepository.GetJobsToProcess(cancellationToken);

        if (waitingJobs.Any())
        {
            waitingJobs.ToList().ForEach(j => jobs.Enqueue(j));
        }
    }

    public Task Enqueue(JobId jobId, CancellationToken cancellationToken)
    {
        jobs.Enqueue(jobId);
        Task.Run(() => Process(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public async Task Process(CancellationToken cancellationToken)
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
                if (jobs.TryDequeue(out var jobId))
                {
                    await ProcessJob(jobId, cancellationToken);
                }

                if (jobs.IsEmpty)
                {
                    await CheckWaiting(cancellationToken);
                }

            } while (!jobs.IsEmpty);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task ProcessJob(JobId jobId, CancellationToken cancellationToken)
    {
        InputDocument inputDocument = null;
        try
        {
            inputDocument = await jobRepository.StartProcessingJob(jobId, cancellationToken);

            if (inputDocument == null)
            {
                return;
            }

            var processor = templateProcessorFactory.GetProcessor(inputDocument.ProcessorId);
            var template = await templateRepository.GetContent(inputDocument.TemplateId, cancellationToken);

            if (!processor.CanProcess(inputDocument.TargetType))
            {
                throw new DocumentProcessingException(inputDocument.Id, $"Processor {processor.Id} {processor.Name} cannot process document type {inputDocument.TargetType.Name}");
            }

            var document = await processor.Generate(template, inputDocument.Data, inputDocument.TargetType, cancellationToken);

            foreach (var targetId in inputDocument.TargetIds)
            {
                var sendDocument = SendDocument.Create(inputDocument);
                sendDocument.Processed = DateTime.UtcNow;
                sendDocument.TargetId = targetId;
                await jobRepository.SendDocument(sendDocument, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            if (inputDocument is null)
            {
                logger.DocumentProcessingError(jobId, exception);
            }

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
