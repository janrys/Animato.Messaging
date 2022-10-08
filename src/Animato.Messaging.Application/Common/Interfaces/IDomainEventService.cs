namespace Animato.Messaging.Application.Common.Interfaces;

using Animato.Messaging.Domain.Common;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken);
}
