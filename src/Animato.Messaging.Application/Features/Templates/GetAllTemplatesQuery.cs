namespace Animato.Messaging.Application.Features.Templates;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllTemplatesQuery : IRequest<IEnumerable<DocumentTemplateDto>>
{
    public GetAllTemplatesQuery(ClaimsPrincipal user) => User = user;


    public ClaimsPrincipal User { get; }

    public class GetAllTemplatesQueryHandler : IRequestHandler<GetAllTemplatesQuery, IEnumerable<DocumentTemplateDto>>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetAllTemplatesQueryHandler> logger;

        public GetAllTemplatesQueryHandler(ITemplateRepository templateRepository
            , IMapper mapper
            , ILogger<GetAllTemplatesQueryHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DocumentTemplateDto>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var templates = await templateRepository.FindAll(cancellationToken);
                return mapper.Map<IEnumerable<DocumentTemplateDto>>(templates);
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
