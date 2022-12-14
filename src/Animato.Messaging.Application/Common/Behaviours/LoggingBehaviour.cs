namespace Animato.Messaging.Application.Common.Behaviours;

using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Security;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger logger;
    private readonly ICurrentUserService currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.GetUser()?.GetUserId() ?? string.Empty;
        var userName = currentUserService.GetUser()?.GetUserName() ?? string.Empty;
        logger.LogInformation("Request: {RequestName} {UserId} {UserName} {Request}", requestName, userId, userName, request);
        return Task.CompletedTask;
    }
}
