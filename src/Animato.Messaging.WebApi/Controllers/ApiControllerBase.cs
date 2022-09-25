namespace Animato.Messaging.WebApi.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    public ISender Mediator { get; set; }
    public ApiControllerBase(ISender mediator) => Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    public ClaimsPrincipal GetUser() => User;
    public ClaimsPrincipal GetAnonymousUser() => null;
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => Mediator.Send(request, cancellationToken);
}
