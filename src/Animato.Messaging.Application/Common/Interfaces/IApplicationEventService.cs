namespace Animato.Messaging.Application.Common.Interfaces;

using MediatR;

public interface IApplicationEventService
{
    Task Publish(INotification eventData, CancellationToken cancellationToken);
}
