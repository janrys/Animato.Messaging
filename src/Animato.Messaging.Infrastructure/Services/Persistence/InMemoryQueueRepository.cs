namespace Animato.Messaging.Infrastructure.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class InMemoryQueueRepository : IQueueRepository
{
    private readonly List<Queue> queues;
    private readonly List<QueueTemplate> queueTemplates;
    private readonly ILogger<InMemoryQueueRepository> logger;

    public InMemoryQueueRepository(InMemoryDataContext dataContext, ILogger<InMemoryQueueRepository> logger)
    {
        if (dataContext is null)
        {
            throw new ArgumentNullException(nameof(dataContext));
        }

        queues = dataContext.Queues;
        queueTemplates = dataContext.QueueTemplates;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<IEnumerable<Queue>> FindAll(CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(queues.AsEnumerable());
        }
        catch (Exception exception)
        {
            logger.QueuesLoadingError(exception);
            throw;
        }
    }

    public async Task<Queue> GetById(QueueId queueId, CancellationToken cancellationToken)
    {
        var queue = await FindById(queueId, cancellationToken);

        if (queue is null)
        {
            throw new NotFoundException(nameof(Queue), queueId);
        }

        return queue;
    }

    public Task<Queue> FindById(QueueId queueId, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(queues.FirstOrDefault(u => u.Id == queueId));
        }
        catch (Exception exception)
        {
            logger.QueuesLoadingError(exception);
            throw;
        }
    }

    public Task<Queue> Create(Queue queue, CancellationToken cancellationToken)
    {
        if (queue is null)
        {
            throw new ArgumentNullException(nameof(queue));
        }

        if (queue.Id == QueueId.Empty)
        {
            queue.Id = QueueId.New();
        }

        try
        {
            queues.Add(queue);
            return Task.FromResult(queue);
        }
        catch (Exception exception)
        {
            logger.QueuesCreatingError(exception);
            throw;
        }
    }

    public Task<Queue> Update(Queue queue, CancellationToken cancellationToken)
    {
        if (queue is null)
        {
            throw new ArgumentNullException(nameof(queue));
        }

        try
        {
            var storedQueue = queues.FirstOrDefault(a => a.Id == queue.Id);

            if (storedQueue == null)
            {
                throw new NotFoundException(nameof(Application), queue.Id);
            }

            queues.Remove(storedQueue);
            queues.Add(queue);

            return Task.FromResult(queue);
        }
        catch (Exception exception)
        {
            logger.QueuesUpdatingError(exception);
            throw;
        }
    }

    public Task Delete(QueueId queueId, CancellationToken cancellationToken)
    {
        try
        {
            queueTemplates.RemoveAll(q => q.QueueId == queueId);
            return Task.FromResult(queues.RemoveAll(q => q.Id == queueId));
        }
        catch (Exception exception)
        {
            logger.QueuesDeletingError(exception);
            throw;
        }
    }

    public Task Clear(CancellationToken cancellationToken)
    {
        queueTemplates.Clear();
        queues.Clear();
        return Task.CompletedTask;
    }

    public async Task<Queue> GetByName(string queueName, CancellationToken cancellationToken)
    {
        var queue = await FindByName(queueName, cancellationToken);

        if (queue is null)
        {
            throw new NotFoundException(nameof(Queue), queueName);
        }

        return queue;
    }
    public Task<Queue> FindByName(string queueName, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(queues.FirstOrDefault(u => u.Name.Equals(queueName, StringComparison.OrdinalIgnoreCase)));
        }
        catch (Exception exception)
        {
            logger.QueuesLoadingError(exception);
            throw;
        }
    }
}
