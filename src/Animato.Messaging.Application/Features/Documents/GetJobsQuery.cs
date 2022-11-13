namespace Animato.Messaging.Application.Features.Documents;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Domain.Enums;
using Animato.Messaging.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetJobsQuery : IRequest<IEnumerable<JobStatusDto>>
{
    public GetJobsQuery(ClaimsPrincipal user) => User = user;

    public ClaimsPrincipal User { get; }


    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<JobStatusDto>>
    {
        private readonly IJobRepository jobRepository;
        private readonly ILogger<GetJobsQueryHandler> logger;

        public GetJobsQueryHandler(IJobRepository jobRepository
            , ILogger<GetJobsQueryHandler> logger)
        {
            this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<JobStatusDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var jobs = new List<JobStatusDto>();

                foreach (var queueType in QueueType.List)
                {
                    jobs.AddRange(await jobRepository.GetStatus(queueType, cancellationToken));
                }

                return jobs;
            }
            catch (BaseException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesCreatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorCreatingQueues, exception);
            }
        }
    }
}

