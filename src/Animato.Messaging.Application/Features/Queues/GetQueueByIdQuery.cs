namespace Animato.Messaging.Application.Features.Queues;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Sso.Application.Common.Logging;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetQueueByIdQuery : IRequest<Queue>
{
    public GetQueueByIdQuery(QueueId queueId, ClaimsPrincipal user)
    {
        QueueId = queueId;
        User = user;
    }

    public QueueId QueueId { get; }
    public ClaimsPrincipal User { get; }

    public class GetQueueByIdQueryValidator : AbstractValidator<GetQueueByIdQuery>
    {
        public GetQueueByIdQueryValidator()
            => RuleFor(v => v.QueueId).NotEmpty().WithMessage(v => $"{nameof(v.QueueId)} must have a value");
    }

    public class GetQueueByIdQueryHandler : IRequestHandler<GetQueueByIdQuery, Queue>
    {
        private readonly IQueueRepository queueRepository;
        private readonly ILogger<GetQueueByIdQueryHandler> logger;

        public GetQueueByIdQueryHandler(IQueueRepository queueRepository, ILogger<GetQueueByIdQueryHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Queue> Handle(GetQueueByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await queueRepository.GetById(request.QueueId, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingQueues, exception);
            }
        }
    }

}
