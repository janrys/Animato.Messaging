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

public class UpdateDocumentTemplateCommand : IRequest<DocumentTemplateDto>
{
    public UpdateDocumentTemplateCommand(DocumentTemplateId templateId
        , CreateDocumentTemplateModel template
        , ClaimsPrincipal user)
    {
        TemplateId = templateId;
        Template = template;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public CreateDocumentTemplateModel Template { get; }
    public ClaimsPrincipal User { get; }

    public class UpdateDocumentTemplateCommandValidator : AbstractValidator<UpdateDocumentTemplateCommand>
    {
        public UpdateDocumentTemplateCommandValidator()
        {
            RuleFor(v => v.TemplateId).NotNull().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
            RuleFor(v => v.Template).NotNull().WithMessage(v => $"{nameof(v.Template)} must have a value");
            RuleFor(v => v.Template).SetValidator(new CreateDocumentTemplateModelValidator());
        }
    }

    public class UpdateDocumentTemplateCommandHandler : IRequestHandler<UpdateDocumentTemplateCommand, DocumentTemplateDto>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplateProcessorFactory templateProcessorFactory;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateDocumentTemplateCommandHandler> logger;

        public UpdateDocumentTemplateCommandHandler(ITemplateRepository templateRepository
            , ITemplateProcessorFactory templateProcessorFactory
            , IMapper mapper
            , ILogger<UpdateDocumentTemplateCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DocumentTemplateDto> Handle(UpdateDocumentTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetById(request.TemplateId, cancellationToken);
            var templateProcessor = templateProcessorFactory.GetProcessor(new ProcessorId(Guid.Parse(request.Template.ProcessorId)));
            templateProcessor.ThrowExceptionIfCannotProcess(template);

            try
            {
                template = mapper.Map(request.Template, template);
                template = await templateRepository.Update(template, cancellationToken);
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
