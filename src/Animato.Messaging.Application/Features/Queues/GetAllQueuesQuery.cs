namespace Animato.Messaging.Application.Features.Queues;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllQueuesQuery : IRequest<IEnumerable<QueueDto>>
{
    public GetAllQueuesQuery(ClaimsPrincipal user) => User = user;


    public ClaimsPrincipal User { get; }

    public class GetAllQueuesQueryHandler : IRequestHandler<GetAllQueuesQuery, IEnumerable<QueueDto>>
    {
        private readonly IQueueRepository queueRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAllQueuesQueryHandler> logger;

        public GetAllQueuesQueryHandler(IQueueRepository queueRepository
            , IMapper mapper
            , ILogger<GetAllQueuesQueryHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<QueueDto>> Handle(GetAllQueuesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var queues = await queueRepository.FindAll(cancellationToken);
                return mapper.Map<IEnumerable<QueueDto>>(queues);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingQueues, exception);
            }
        }
    }

}
