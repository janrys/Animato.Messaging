namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Application.Features.TemplateProcessors;
using Animato.Messaging.Application.Features.TemplateProcessors.Contracts;

[Route("api/template_processor")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class TemplateProcessorController : ApiControllerBase
{
    public TemplateProcessorController(ISender mediator) : base(mediator) { }

    /// <summary>
    /// Get all registered template processors
    /// </summary>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>List of template processors</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RegisteredTemplateProcessor>))]
    [HttpGet(Name = "GetProcessors")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTemplateProcessorsQuery(GetUser());
        var processors = await Send(query, cancellationToken);
        return Ok(processors);
    }
}
