namespace Animato.Messaging.Application.Features.Queues;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateDocumentTemplateCommand : IRequest<DocumentTemplateDto>
{
    public CreateDocumentTemplateCommand(CreateDocumentTemplateModel template, ClaimsPrincipal user)
    {
        Template = template;
        User = user;
    }

    public CreateDocumentTemplateModel Template { get; }
    public ClaimsPrincipal User { get; }

    public class CreateDocumentTemplateCommandValidator : AbstractValidator<CreateDocumentTemplateCommand>
    {
        public CreateDocumentTemplateCommandValidator()
        {
            RuleFor(v => v.Template).NotNull().WithMessage(v => $"{nameof(v.Template)} must have a value");
            RuleFor(v => v.Template).SetValidator(new CreateDocumentTemplateModelValidator());
        }
    }

    public class CreateDocumentTemplateCommandHandler : IRequestHandler<CreateDocumentTemplateCommand, DocumentTemplateDto>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplateProcessorFactory templateProcessorFactory;
        private readonly IMapper mapper;
        private readonly ILogger<CreateDocumentTemplateCommandHandler> logger;

        public CreateDocumentTemplateCommandHandler(ITemplateRepository templateRepository
            , ITemplateProcessorFactory templateProcessorFactory
            , IMapper mapper
            , ILogger<CreateDocumentTemplateCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DocumentTemplateDto> Handle(CreateDocumentTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = mapper.Map<DocumentTemplate>(request.Template);
            var templateProcessor = templateProcessorFactory.GetProcessor(new ProcessorId(Guid.Parse(request.Template.ProcessorId)));
            templateProcessor.ThrowExceptionIfCannotProcess(template);

            try
            {
                template = await templateRepository.Create(template, cancellationToken);
                return mapper.Map<DocumentTemplateDto>(template);
            }
            catch (BaseException) { throw; }
            catch (Exception exception)
            {
                logger.TemplatesCreatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorCreatingTemplates, exception);
            }
        }
    }

}

