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

public class CreateQueueCommand : IRequest<QueueDto>
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
            RuleFor(v => v.Queue).SetValidator(new CreateQueueModelValidator());
        }
    }

    public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, QueueDto>
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

        public async Task<QueueDto> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var queue = mapper.Map<Queue>(request.Queue);
                queue = await queueRepository.Create(queue, cancellationToken);
                return mapper.Map<QueueDto>(queue);
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

