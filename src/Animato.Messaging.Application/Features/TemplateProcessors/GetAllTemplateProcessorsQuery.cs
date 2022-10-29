namespace Animato.Messaging.Application.Features.TemplateProcessors;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.TemplateProcessors.Contracts;
using Animato.Messaging.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllTemplateProcessorsQuery : IRequest<IEnumerable<RegisteredTemplateProcessor>>
{
    public GetAllTemplateProcessorsQuery(ClaimsPrincipal user) => User = user;

    public ClaimsPrincipal User { get; }

    public class GetAllTemplateProcessorsQueryHandler : IRequestHandler<GetAllTemplateProcessorsQuery, IEnumerable<RegisteredTemplateProcessor>>
    {
        private readonly IMapper mapper;
        private readonly ITemplateProcessorFactory templateProcessorFactory;
        private readonly ILogger<GetAllTemplateProcessorsQueryHandler> logger;

        public GetAllTemplateProcessorsQueryHandler(IMapper mapper
            , ITemplateProcessorFactory templateProcessorFactory
            , ILogger<GetAllTemplateProcessorsQueryHandler> logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IEnumerable<RegisteredTemplateProcessor>> Handle(GetAllTemplateProcessorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(mapper.Map<IEnumerable<RegisteredTemplateProcessor>>(templateProcessorFactory.Processors));
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
