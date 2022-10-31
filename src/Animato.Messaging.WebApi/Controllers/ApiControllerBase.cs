namespace Animato.Messaging.WebApi.Controllers;

using System.Security.Claims;
using Animato.Messaging.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    public ISender Mediator { get; set; }
    public ApiControllerBase(ISender mediator) => Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    protected ClaimsPrincipal GetUser() => User;
    protected ClaimsPrincipal GetAnonymousUser() => null;
    protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => Mediator.Send(request, cancellationToken);

    protected bool TryParseAndValidateQueueId(string id, out QueueId queueId, out IActionResult errorActionResult)
    {
        errorActionResult = null;
        queueId = default;

        if (id is null)
        {
            errorActionResult = BadRequest($"Queue {nameof(id)} must have a value");
            return false;
        }

        if (Guid.TryParse(id, out var parsedQueueId))
        {
            queueId = new QueueId(parsedQueueId);
        }
        else
        {
            errorActionResult = BadRequest($"Queue {nameof(id)} has a wrong format '{id}'");
            return false;
        }

        return true;
    }

    protected bool TryParseAndValidateTemplateId(string id, out DocumentTemplateId templateId, out IActionResult errorActionResult)
    {
        errorActionResult = null;
        templateId = default;

        if (id is null)
        {
            errorActionResult = BadRequest($"Template {nameof(id)} must have a value");
            return false;
        }

        if (Guid.TryParse(id, out var parsedQueueId))
        {
            templateId = new DocumentTemplateId(parsedQueueId);
        }
        else
        {
            errorActionResult = BadRequest($"Template {nameof(id)} has a wrong format '{id}'");
            return false;
        }

        return true;
    }
}
