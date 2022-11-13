namespace Animato.Messaging.Application.Features.Documents;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetJobByIdQuery : IRequest<JobStatusDto>
{
    public GetJobByIdQuery(JobId jobId, ClaimsPrincipal user)
    {
        JobId = jobId;
        User = user;
    }

    public JobId JobId { get; }
    public ClaimsPrincipal User { get; }

    public class GetJobByIdQueryValidator : AbstractValidator<GetJobByIdQuery>
    {
        public GetJobByIdQueryValidator()
            => RuleFor(v => v.JobId).NotNull().WithMessage(v => $"{nameof(v.JobId)} must have a value");
    }

    public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobStatusDto>
    {
        private readonly IJobRepository jobRepository;
        private readonly ILogger<GetJobByIdQueryHandler> logger;

        public GetJobByIdQueryHandler(IJobRepository jobRepository
            , ILogger<GetJobByIdQueryHandler> logger)
        {
            this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<JobStatusDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await jobRepository.GetStatus(request.JobId, cancellationToken);
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

