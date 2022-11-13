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
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetJobsByQueueTypeQuery : IRequest<IEnumerable<JobStatusDto>>
{
    public GetJobsByQueueTypeQuery(QueueType queueType, ClaimsPrincipal user)
    {
        QueueType = queueType;
        User = user;
    }

    public QueueType QueueType { get; }
    public ClaimsPrincipal User { get; }

    public class GetJobsByQueueTypeQueryValidator : AbstractValidator<GetJobsByQueueTypeQuery>
    {
        public GetJobsByQueueTypeQueryValidator()
            => RuleFor(v => v.QueueType).NotNull().WithMessage(v => $"{nameof(v.QueueType)} must have a value");
    }

    public class GetJobsByQueueTypeQueryHandler : IRequestHandler<GetJobsByQueueTypeQuery, IEnumerable<JobStatusDto>>
    {
        private readonly IJobRepository jobRepository;
        private readonly ILogger<GetJobsByQueueTypeQueryHandler> logger;

        public GetJobsByQueueTypeQueryHandler(IJobRepository jobRepository
            , ILogger<GetJobsByQueueTypeQueryHandler> logger)
        {
            this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<JobStatusDto>> Handle(GetJobsByQueueTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await jobRepository.GetStatus(request.QueueType, cancellationToken);
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

