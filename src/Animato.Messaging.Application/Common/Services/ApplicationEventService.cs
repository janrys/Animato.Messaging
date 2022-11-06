namespace Animato.Messaging.Application.Common.Services;
using System;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

public class ApplicationEventService : IApplicationEventService
{
    private readonly IPublisher mediator;
    private readonly ILogger<ApplicationEventService> logger;

    public ApplicationEventService(IPublisher mediator, ILogger<ApplicationEventService> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Publish(INotification eventData, CancellationToken cancellationToken)
    {
        logger.ApplicationEventSendDebug(eventData);
        return mediator.Publish(eventData, cancellationToken);
    }
}
