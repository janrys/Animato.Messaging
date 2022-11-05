namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Application.Features.Documents;

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class JobController : ApiControllerBase
{
    public JobController(ISender mediator) : base(mediator) { }

    /// <summary>
    /// Create document
    /// </summary>
    /// <param name="document"></param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Created document status</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDto))]
    [HttpPost(Name = "CreateJob")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobModel document, CancellationToken cancellationToken)
    {
        if (document is null)
        {
            return BadRequest($"{nameof(document)} must have a value");
        }

        var command = new CreateJobCommand(document, GetUser());
        var job = await Send(command, cancellationToken);
        return Ok(job);
    }
}
