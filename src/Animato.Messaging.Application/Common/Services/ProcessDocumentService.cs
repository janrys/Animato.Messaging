namespace Animato.Messaging.Application.Common.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class ProcessDocumentService : IProcessDocumentService
{
    private readonly ConcurrentQueue<JobId> jobs = new();
    private readonly IJobRepository jobRepository;
    private readonly ILogger<ProcessDocumentService> logger;
    private readonly SemaphoreSlim semaphore = new(1, 1);

    public ProcessDocumentService(IJobRepository jobRepository
        , ILogger<ProcessDocumentService> logger)
    {
        this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
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
        if (semaphore.CurrentCount == 0)
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
                    await ProcessJob(jobId);
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

    private Task ProcessJob(JobId jobId) => throw new NotImplementedException();
}
