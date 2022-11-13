namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Application.Features.Documents;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

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

    /// <summary>
    /// Get jobs and their statuses
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="queueType"></param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>List of job statuses</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobStatusDto))]
    [HttpGet(Name = "GetJobs")]
    public async Task<IActionResult> GetJobs(string jobId, string queueType, CancellationToken cancellationToken)
    {
        var jobs = new List<JobStatusDto>();
        if (!string.IsNullOrEmpty(jobId))
        {
            if (!Guid.TryParse(jobId, out var parsedJobId))
            {
                return BadRequest();
            }

            var command = new GetJobByIdQuery(new JobId(parsedJobId), GetUser());
            var job = await Send(command, cancellationToken);
            jobs = new List<JobStatusDto>();

            if (job is not null)
            {
                jobs.Add(job);
            }
        }
        else if (!string.IsNullOrEmpty(queueType))
        {
            if (!QueueType.TryFromName(queueType, out var parsedQueueType))
            {
                return BadRequest();
            }

            var command = new GetJobsByQueueTypeQuery(parsedQueueType, GetUser());
            jobs.AddRange(await Send(command, cancellationToken));
        }
        else
        {
            var command = new GetJobsQuery(GetUser());
            jobs.AddRange(await Send(command, cancellationToken));
        }

        return Ok(jobs);
    }
}
