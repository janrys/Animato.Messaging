namespace Animato.Messaging.Application.Features.Queues;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class DeleteQueueCommand : IRequest<Unit>
{
    public DeleteQueueCommand(QueueId queueId, ClaimsPrincipal user)
    {
        QueueId = queueId;
        User = user;
    }

    public QueueId QueueId { get; }
    public ClaimsPrincipal User { get; }

    public class DeleteQueueCommandValidator : AbstractValidator<DeleteQueueCommand>
    {
        public DeleteQueueCommandValidator()
            => RuleFor(v => v.QueueId).NotNull().WithMessage(v => $"{nameof(v.QueueId)} must have a value");
    }

    public class DeleteQueueCommandHandler : IRequestHandler<DeleteQueueCommand, Unit>
    {
        private readonly IQueueRepository queueRepository;
        private readonly ILogger<DeleteQueueCommandHandler> logger;

        public DeleteQueueCommandHandler(IQueueRepository queueRepository
            , ILogger<DeleteQueueCommandHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
        {
            var queue = await queueRepository.FindById(request.QueueId, cancellationToken);

            if (queue is null)
            {
                return Unit.Value;
            }

            try
            {
                await queueRepository.Delete(request.QueueId, cancellationToken);
                return Unit.Value;
            }
            catch (BaseException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesDeletingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorDeletingQueues, exception);
            }
        }
    }

}
