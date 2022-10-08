namespace Animato.Messaging.Application.Features.Templates;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetTemplateByIdQuery : IRequest<DocumentTemplate>
{
    public GetTemplateByIdQuery(DocumentTemplateId templateId, ClaimsPrincipal user)
    {
        TemplateId = templateId;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public ClaimsPrincipal User { get; }

    public class GetTemplateByIdQueryValidator : AbstractValidator<GetTemplateByIdQuery>
    {
        public GetTemplateByIdQueryValidator()
            => RuleFor(v => v.TemplateId).NotEmpty().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
    }

    public class GetTemplateByIdQueryHandler : IRequestHandler<GetTemplateByIdQuery, DocumentTemplate>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ILogger<GetTemplateByIdQueryHandler> logger;

        public GetTemplateByIdQueryHandler(ITemplateRepository templateRepository, ILogger<GetTemplateByIdQueryHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DocumentTemplate> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await templateRepository.GetById(request.TemplateId, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.QueuesLoadingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorLoadingQueues, exception);
            }
        }
    }

}
