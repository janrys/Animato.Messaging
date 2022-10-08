namespace Animato.Messaging.Infrastructure.Services.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Common;

public class NullDomainEventService : IDomainEventService
{
    public Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken) => Task.CompletedTask;
}
