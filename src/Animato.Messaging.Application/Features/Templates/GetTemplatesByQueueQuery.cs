namespace Animato.Messaging.Application.Features.Templates;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetTemplatesByQueueQuery : IRequest<IEnumerable<DocumentTemplateDto>>
{
    public GetTemplatesByQueueQuery(QueueId queueId, ClaimsPrincipal user)
    {
        QueueId = queueId;
        User = user;
    }

    public QueueId QueueId { get; }
    public ClaimsPrincipal User { get; }

    public class GetTemplatesByQueueQueryValidator : AbstractValidator<GetTemplatesByQueueQuery>
    {
        public GetTemplatesByQueueQueryValidator()
            => RuleFor(v => v.QueueId).NotEmpty().WithMessage(v => $"{nameof(v.QueueId)} must have a value");
    }

    public class GetTemplatesByQueueQueryHandler : IRequestHandler<GetTemplatesByQueueQuery, IEnumerable<DocumentTemplateDto>>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetTemplatesByQueueQueryHandler> logger;

        public GetTemplatesByQueueQueryHandler(ITemplateRepository templateRepository
            , IMapper mapper
            , ILogger<GetTemplatesByQueueQueryHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DocumentTemplateDto>> Handle(GetTemplatesByQueueQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var templates = await templateRepository.FindByQueue(request.QueueId, cancellationToken);
                return mapper.Map<IEnumerable<DocumentTemplateDto>>(templates);
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingQueues, exception);
            }
        }
    }

}
