namespace Animato.Messaging.Infrastructure.AzureStorage.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Infrastructure.AzureStorage.Services.Persistence.DTOs;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

public class AzureTableQueueRepository : IQueueRepository
{
    private TableClient TableQueues => dataContext.Queues;

    private Func<CancellationToken, Task> CheckIfTableExists => dataContext.ThrowExceptionIfTableNotExists;
    private readonly AzureTableStorageDataContext dataContext;
    private readonly ILogger<AzureTableQueueRepository> logger;

    public AzureTableQueueRepository(AzureTableStorageDataContext dataContext
        , ILogger<AzureTableQueueRepository> logger)
    {
        this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private Task ThrowExceptionIfTableNotExists(CancellationToken cancellationToken)
        => CheckIfTableExists(cancellationToken);

    public async Task<IEnumerable<Queue>> GetAll(CancellationToken cancellationToken)
    {
        await ThrowExceptionIfTableNotExists(cancellationToken);

        try
        {
            var results = new List<QueueTableEntity>();
            var queryResult = TableQueues.QueryAsync<QueueTableEntity>(cancellationToken: cancellationToken, maxPerPage: AzureTableStorageDataContext.MAX_PER_PAGE);
            await queryResult.AsPages()
                .ForEachAsync(page => results.AddRange(page.Values), cancellationToken)
                .ConfigureAwait(false);

            return results.Select(e => e.ToEntity());
        }
        catch (Exception exception)
        {
            logger.QueuesLoadingError(exception);
            throw;
        }
    }

    public async Task<Queue> GetById(QueueId queueId, CancellationToken cancellationToken)
    {
        await ThrowExceptionIfTableNotExists(cancellationToken);

        try
        {
            var results = new List<QueueTableEntity>();
            var queryResult = TableQueues.QueryAsync<QueueTableEntity>(a => a.RowKey == queueId.Value.ToString(), cancellationToken: cancellationToken);

            await queryResult.AsPages()
                .ForEachAsync(page => results.AddRange(page.Values), cancellationToken)
                .ConfigureAwait(false);

            if (results.Count == 1)
            {
                return results.First().ToEntity();
            }

            if (results.Count == 0)
            {
                return null;
            }

            throw new DataAccessException($"Found duplicate applications ({results.Count}) for id {queueId.Value}");
        }
        catch (Exception exception)
        {
            logger.QueuesLoadingError(exception);
            throw;
        }
    }


    public async Task<Queue> Create(Queue queue, CancellationToken cancellationToken)
    {
        if (queue is null)
        {
            throw new ArgumentNullException(nameof(queue));
        }

        await ThrowExceptionIfTableNotExists(cancellationToken);


        if (queue.Id == QueueId.Empty)
        {
            queue.Id = QueueId.New();
        }

        try
        {
            var tableEntity = queue.ToTableEntity();
            await TableQueues.AddEntityAsync(tableEntity, cancellationToken);
            return queue;
        }
        catch (Exception exception)
        {
            logger.QueuesCreatingError(exception);
            throw;
        }
    }

    public async Task<Queue> Update(Queue queue, CancellationToken cancellationToken)
    {
        if (queue is null)
        {
            throw new ArgumentNullException(nameof(queue));
        }

        await ThrowExceptionIfTableNotExists(cancellationToken);

        try
        {
            var tableEntity = queue.ToTableEntity();
            await TableQueues.UpdateEntityAsync(tableEntity, Azure.ETag.All, cancellationToken: cancellationToken);
            return queue;
        }
        catch (Exception exception)
        {
            logger.QueuesUpdatingError(exception);
            throw;
        }
    }

    public async Task Delete(QueueId queueId, CancellationToken cancellationToken)
    {
        await ThrowExceptionIfTableNotExists(cancellationToken);

        var application = await GetById(queueId, cancellationToken);

        if (application == null)
        {
            return;
        }

        var tableEntity = application.ToTableEntity();

        try
        {
            await TableQueues.DeleteEntityAsync(tableEntity.PartitionKey, tableEntity.RowKey, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.QueuesDeletingError(exception);
            throw;
        }
    }
    public async Task Clear(CancellationToken cancellationToken)
    {
        await ThrowExceptionIfTableNotExists(cancellationToken);

        try
        {
            await AzureTableStorageDataContext.DeleteAllEntitiesAsync(TableQueues, CancellationToken.None);
        }
        catch (Exception exception)
        {
            logger.QueuesDeletingError(exception);
            throw;
        }
    }
}
