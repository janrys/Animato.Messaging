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

public class DeleteDocumentTemplateCommand : IRequest<Unit>
{
    public DeleteDocumentTemplateCommand(DocumentTemplateId templateId, ClaimsPrincipal user)
    {
        TemplateId = templateId;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public ClaimsPrincipal User { get; }

    public class DeleteDocumentTemplateCommandValidator : AbstractValidator<DeleteDocumentTemplateCommand>
    {
        public DeleteDocumentTemplateCommandValidator()
            => RuleFor(v => v.TemplateId).NotNull().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
    }

    public class DeleteDocumentTemplateCommandHandler : IRequestHandler<DeleteDocumentTemplateCommand, Unit>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ILogger<DeleteDocumentTemplateCommandHandler> logger;

        public DeleteDocumentTemplateCommandHandler(ITemplateRepository templateRepository
            , ILogger<DeleteDocumentTemplateCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteDocumentTemplateCommand request, CancellationToken cancellationToken)
        {
            var application = await templateRepository.GetById(request.TemplateId, cancellationToken);

            if (application is null)
            {
                return Unit.Value;
            }

            try
            {
                await templateRepository.Delete(request.TemplateId, cancellationToken);
                return Unit.Value;
            }
            catch (Exceptions.ValidationException) { throw; }
            catch (Exception exception)
            {
                logger.TemplatesDeletingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorDeletingTemplates, exception);
            }
        }
    }

}
