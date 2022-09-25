namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Application.Features.Queues;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
    [HttpGet("{id}", Name = "GetQueueById")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }
        QueueId queueId;
        if (Guid.TryParse(id, out var parsedQueueId))
        {
            queueId = new QueueId(parsedQueueId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
    [HttpPut("{id}", Name = "UpdateQueue")]
    public async Task<IActionResult> UpdateQueue(string id, [FromBody] CreateQueueModel queue, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }

        QueueId queueId;
        if (Guid.TryParse(id, out var parsedQueueId))
        {
            queueId = new QueueId(parsedQueueId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
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
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }

        QueueId queueId;
        if (Guid.TryParse(id, out var parsedQueueId))
        {
            queueId = new QueueId(parsedQueueId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
        }

        var command = new DeleteQueueCommand(queueId, GetUser());
        await Send(command, cancellationToken);
        return Ok();
    }
}
