namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Application.Features.Queues;
using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Application.Features.Templates;

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class QueueController : ApiControllerBase
{
    public QueueController(ISender mediator) : base(mediator) { }

    /// <summary>
    /// Get all queues
    /// </summary>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>List of queues</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Queue>))]
    [HttpGet(Name = "GetQueues")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllQueuesQuery(GetUser());
        var queues = await Send(query, cancellationToken);
        return Ok(queues);
    }

    /// <summary>
    /// Get queue
    /// </summary>
    /// <param name="id">Queue id</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Queue</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueueDto))]
    [HttpGet("{id}", Name = "GetQueueById")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        var query = new GetQueueByIdQuery(queueId, GetUser());
        var queue = await Send(query, cancellationToken);

        if (queue is null)
        {
            return NotFound();
        }

        return Ok(queue);
    }

    /// <summary>
    /// Create queue
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Created queue</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueueDto))]
    [HttpPost(Name = "CreateQueue")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateQueueModel queue, CancellationToken cancellationToken)
    {
        if (queue is null)
        {
            return BadRequest($"{nameof(queue)} must have a value");
        }

        var command = new CreateQueueCommand(queue, GetUser());
        var createdQueue = await Send(command, cancellationToken);
        return Ok(createdQueue);
    }

    /// <summary>
    /// Update queue
    /// </summary>
    /// <param name="id">Queue id to update</param>
    /// <param name="queue">Queue changes</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Updated queue</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueueDto))]
    [HttpPut("{id}", Name = "UpdateQueue")]
    public async Task<IActionResult> UpdateQueue(string id, [FromBody] CreateQueueModel queue, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        if (queue is null)
        {
            return BadRequest($"{nameof(queue)} must have a value");
        }

        var command = new UpdateQueueCommand(queueId, queue, GetUser());
        var updatedQueue = await Send(command, cancellationToken);
        return Ok(updatedQueue);
    }

    /// <summary>
    /// Delete queue
    /// </summary>
    /// <param name="id">Queue id to delete</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id}", Name = "DeleteQueue")]
    public async Task<IActionResult> DeleteQueue(string id, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        var command = new DeleteQueueCommand(queueId, GetUser());
        await Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Get templates allowed for queue
    /// </summary>
    /// <param name="id">Queue id</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Allowed template list</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentTemplateDto>))]
    [HttpPost("{id}/template", Name = "GetTemplatesByQueue")]
    public async Task<IActionResult> GetTemplatesByQueue(string id, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        var command = new GetTemplatesByQueueQuery(queueId, GetUser());
        var templates = await Send(command, cancellationToken);
        return Ok(templates);
    }

    /// <summary>
    /// Add template to queue
    /// </summary>
    /// <param name="id">Queue id</param>
    /// <param name="templateId">Template id</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Allowed template list</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentTemplateDto>))]
    [HttpPost("{id}/template/{templateId}", Name = "AddTemplateToQueue")]
    public async Task<IActionResult> AddTemplateToQueue(string id, string templateId, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        if (!TryParseAndValidateTemplateId(templateId, out var templateIdValidated, out var actionResultTemplate))
        {
            return actionResultTemplate;
        }

        var command = new AddTemplateToQueueCommand(queueId, templateIdValidated, GetUser());
        var templates = await Send(command, cancellationToken);
        return Ok(templates);
    }

    /// <summary>
    /// Remove template from queue
    /// </summary>
    /// <param name="id">Queue id</param>
    /// <param name="templateId">Template id</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Allowed template list</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id}/template/{templateId}", Name = "RemoveTemplateFromQueue")]
    public async Task<IActionResult> RemoveTemplateFromQueue(string id, string templateId, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateQueueId(id, out var queueId, out var actionResult))
        {
            return actionResult;
        }

        if (!TryParseAndValidateTemplateId(templateId, out var templateIdValidated, out var actionResultTemplate))
        {
            return actionResultTemplate;
        }

        var command = new RemoveTemplateFromQueueCommand(queueId, templateIdValidated, GetUser());
        var templates = await Send(command, cancellationToken);
        return Ok(templates);
    }
}
