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
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllQueuesQuery : IRequest<IEnumerable<Queue>>
{
    public GetAllQueuesQuery(ClaimsPrincipal user) => User = user;


    public ClaimsPrincipal User { get; }

    public class GetAllQueuesQueryHandler : IRequestHandler<GetAllQueuesQuery, IEnumerable<Queue>>
    {
        private readonly IQueueRepository queueRepository;
        private readonly ILogger<GetAllQueuesQueryHandler> logger;

        public GetAllQueuesQueryHandler(IQueueRepository queueRepository, ILogger<GetAllQueuesQueryHandler> logger)
        {
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Queue>> Handle(GetAllQueuesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await queueRepository.GetAll(cancellationToken);
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
