namespace Animato.Messaging.Application.Common.Behaviours;

using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Security;
using Animato.Messaging.Application.Exceptions;
using MediatR;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService currentUserService;
    private readonly IAuthorizationService authorizationService;

    public AuthorizationBehaviour(
        ICurrentUserService currentUserService, IAuthorizationService authorizationService)
    {
        this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!await authorizationService.IsAllowed(request, currentUserService.GetUser()))
        {
            throw new ForbiddenAccessException(currentUserService.GetUser()?.GetUserName(), typeof(TRequest).Name);
        }

        return await next();
    }
}
