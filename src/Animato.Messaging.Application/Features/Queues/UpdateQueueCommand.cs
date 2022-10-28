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
using Animato.Messaging.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateQueueCommand : IRequest<QueueDto>
{
    public UpdateQueueCommand(QueueId queueId
        , CreateQueueModel queue
        , ClaimsPrincipal user)
    {
        QueueId = queueId;
        Queue = queue;
        User = user;
    }

    public QueueId QueueId { get; }
    public CreateQueueModel Queue { get; }
    public ClaimsPrincipal User { get; }

    public class UpdateQueueCommandValidator : AbstractValidator<UpdateQueueCommand>
    {
        public UpdateQueueCommandValidator()
        {
            RuleFor(v => v.Queue).NotNull().WithMessage(v => $"{nameof(v.Queue)} must have a value");
            RuleFor(v => v.Queue).SetValidator(new CreateQueueModelValidator());
        }
    }

    public class UpdateQueueCommandHandler : IRequestHandler<UpdateQueueCommand, QueueDto>
    {
        private readonly IQueueRepository queueRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateQueueCommandHandler> logger;

        public UpdateQueueCommandHandler(IQueueRepository queueRepository
            , IMapper mapper
            , ILogger<UpdateQueueCommandHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<QueueDto> Handle(UpdateQueueCommand request, CancellationToken cancellationToken)
        {
            var queue = await queueRepository.GetById(request.QueueId, cancellationToken);

            try
            {
                queue = mapper.Map(request.Queue, queue);
                queue = await queueRepository.Update(queue, cancellationToken);
                return mapper.Map<QueueDto>(queue);
            }
            catch (BaseException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesUpdatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorUpdatingQueues, exception);
            }
        }
    }

}
