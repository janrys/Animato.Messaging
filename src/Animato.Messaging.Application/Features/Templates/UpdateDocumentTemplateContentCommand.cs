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

public class UpdateDocumentTemplateContentCommand : IRequest<DocumentTemplateDto>
{
    public UpdateDocumentTemplateContentCommand(DocumentTemplateId templateId
        , string fileName
        , Stream content
        , ClaimsPrincipal user)
    {
        TemplateId = templateId;
        FileName = fileName;
        Content = content;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public string FileName { get; }
    public Stream Content { get; }
    public ClaimsPrincipal User { get; }

    public class UpdateDocumentTemplateContentCommandValidator : AbstractValidator<UpdateDocumentTemplateContentCommand>
    {
        public UpdateDocumentTemplateContentCommandValidator()
        {
            RuleFor(v => v.TemplateId).NotNull().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
            RuleFor(v => v.FileName).NotEmpty().WithMessage(v => $"{nameof(v.FileName)} must have a value");
            RuleFor(v => v.Content).NotNull().WithMessage(v => $"{nameof(v.Content)} must have a value");
        }
    }

    public class UpdateDocumentTemplateContentCommandHandler : IRequestHandler<UpdateDocumentTemplateContentCommand, DocumentTemplateDto>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateDocumentTemplateContentCommandHandler> logger;

        public UpdateDocumentTemplateContentCommandHandler(ITemplateRepository templateRepository
            , IMapper mapper
            , ILogger<UpdateDocumentTemplateContentCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DocumentTemplateDto> Handle(UpdateDocumentTemplateContentCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetById(request.TemplateId, cancellationToken);

            try
            {
                await templateRepository.UpdateContent(template.Id, request.FileName, request.Content, cancellationToken);
                return mapper.Map<DocumentTemplateDto>(template);
            }
            catch (Exceptions.ValidationException) { throw; }
            catch (Exception exception)
            {
                logger.TemplatesUpdatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorUpdatingTemplates, exception);
            }
        }
    }

}
