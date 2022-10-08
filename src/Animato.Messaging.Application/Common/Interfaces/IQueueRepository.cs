namespace Animato.Messaging.Application.Common.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface IQueueRepository
{
    Task<IEnumerable<Queue>> GetAll(CancellationToken cancellationToken);
    Task<Queue> GetById(QueueId queueId, CancellationToken cancellationToken);
    Task<Queue> Create(Queue queue, CancellationToken cancellationToken);
    Task<Queue> Update(Queue queue, CancellationToken cancellationToken);
    Task Delete(QueueId queueId, CancellationToken cancellationToken);
    Task Clear(CancellationToken cancellationToken);
}
