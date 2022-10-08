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

public class UpdateQueueCommand : IRequest<Queue>
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
            RuleFor(v => v.Queue).InjectValidator();
        }
    }

    public class UpdateQueueCommandHandler : IRequestHandler<UpdateQueueCommand, Queue>
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

        public async Task<Queue> Handle(UpdateQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var queue = await queueRepository.GetById(request.QueueId, cancellationToken);

                if (queue is null)
                {
                    throw new NotFoundException(nameof(Queue), request.QueueId);
                }

                queue = mapper.Map(request.Queue, queue);
                return await queueRepository.Update(queue, cancellationToken);
            }
            catch (NotFoundException) { throw; }
            catch (Exceptions.ValidationException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesUpdatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorUpdatingQueues, exception);
            }
        }
    }

}
