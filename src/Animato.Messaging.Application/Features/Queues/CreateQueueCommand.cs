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

public class CreateQueueCommand : IRequest<Queue>
{
    public CreateQueueCommand(CreateQueueModel queue, ClaimsPrincipal user)
    {
        Queue = queue;
        User = user;
    }

    public CreateQueueModel Queue { get; }
    public ClaimsPrincipal User { get; }

    public class CreateQueueCommandValidator : AbstractValidator<CreateQueueCommand>
    {
        public CreateQueueCommandValidator()
        {
            RuleFor(v => v.Queue).NotNull().WithMessage(v => $"{nameof(v.Queue)} must have a value");
            RuleFor(v => v.Queue).InjectValidator();
        }
    }

    public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, Queue>
    {
        private readonly IQueueRepository queueRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateQueueCommandHandler> logger;

        public CreateQueueCommandHandler(IQueueRepository queueRepository
            , IMapper mapper
            , ILogger<CreateQueueCommandHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Queue> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var queue = mapper.Map<Queue>(request.Queue);
                return await queueRepository.Create(queue, cancellationToken);
            }
            catch (Exceptions.ValidationException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesCreatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorCreatingQueues, exception);
            }
        }
    }

}

