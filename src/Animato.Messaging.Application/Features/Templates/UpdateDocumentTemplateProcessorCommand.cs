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

public class UpdateDocumentTemplateProcessorCommand : IRequest<DocumentTemplateDto>
{
    public UpdateDocumentTemplateProcessorCommand(DocumentTemplateId templateId
        , ProcessorId processorId
        , ClaimsPrincipal user)
    {
        TemplateId = templateId;
        ProcessorId = processorId;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public ProcessorId ProcessorId { get; }
    public ClaimsPrincipal User { get; }

    public class UpdateDocumentTemplateProcessorCommandValidator : AbstractValidator<UpdateDocumentTemplateProcessorCommand>
    {
        public UpdateDocumentTemplateProcessorCommandValidator()
        {
            RuleFor(v => v.TemplateId).NotNull().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
            RuleFor(v => v.ProcessorId).NotNull().WithMessage(v => $"{nameof(v.ProcessorId)} must have a value");
        }
    }

    public class UpdateDocumentTemplateProcessorCommandHandler : IRequestHandler<UpdateDocumentTemplateProcessorCommand, DocumentTemplateDto>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplateProcessorFactory templateProcessorFactory;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateDocumentTemplateProcessorCommandHandler> logger;

        public UpdateDocumentTemplateProcessorCommandHandler(ITemplateRepository templateRepository
            , ITemplateProcessorFactory templateProcessorFactory
            , IMapper mapper
            , ILogger<UpdateDocumentTemplateProcessorCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DocumentTemplateDto> Handle(UpdateDocumentTemplateProcessorCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetById(request.TemplateId, cancellationToken);
            var templateProcessor = templateProcessorFactory.GetProcessor(request.ProcessorId);
            templateProcessor.ThrowExceptionIfCannotProcess(template);

            try
            {
                template.ProcessorId = request.ProcessorId;
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
