namespace Animato.Messaging.Application.Features.Templates;
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

public class GetAllTemplatesQuery : IRequest<IEnumerable<DocumentTemplate>>
{
    public GetAllTemplatesQuery(ClaimsPrincipal user) => User = user;


    public ClaimsPrincipal User { get; }

    public class GetAllTemplatesQueryHandler : IRequestHandler<GetAllTemplatesQuery, IEnumerable<DocumentTemplate>>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ILogger<GetAllTemplatesQueryHandler> logger;

        public GetAllTemplatesQueryHandler(ITemplateRepository templateRepository, ILogger<GetAllTemplatesQueryHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DocumentTemplate>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await templateRepository.FindAll(cancellationToken);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingTemplates, exception);
            }
        }
    }

}
