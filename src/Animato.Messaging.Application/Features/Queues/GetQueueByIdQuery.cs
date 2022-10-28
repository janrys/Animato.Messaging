namespace Animato.Messaging.Application.Features.Queues;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetQueueByIdQuery : IRequest<QueueDto>
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

    public class GetQueueByIdQueryHandler : IRequestHandler<GetQueueByIdQuery, QueueDto>
    {
        private readonly IQueueRepository queueRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetQueueByIdQueryHandler> logger;

        public GetQueueByIdQueryHandler(IQueueRepository queueRepository
            , IMapper mapper
            , ILogger<GetQueueByIdQueryHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<QueueDto> Handle(GetQueueByIdQuery request, CancellationToken cancellationToken)
        {
            var queue = await queueRepository.GetById(request.QueueId, cancellationToken);

            try
            {
                return mapper.Map<QueueDto>(queue);
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingQueues, exception);
            }
        }
    }

}
