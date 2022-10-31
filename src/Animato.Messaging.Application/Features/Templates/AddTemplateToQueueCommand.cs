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

public class AddTemplateToQueueCommand : IRequest<IEnumerable<DocumentTemplateDto>>
{
    public AddTemplateToQueueCommand(QueueId queueId
        , DocumentTemplateId templateId
        , ClaimsPrincipal user)
    {
        TemplateId = templateId;
        QueueId = queueId;
        User = user;
    }

    public DocumentTemplateId TemplateId { get; }
    public QueueId QueueId { get; }
    public ClaimsPrincipal User { get; }

    public class AddTemplateToQueueCommandValidator : AbstractValidator<AddTemplateToQueueCommand>
    {
        public AddTemplateToQueueCommandValidator()
        {
            RuleFor(v => v.TemplateId).NotNull().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
            RuleFor(v => v.QueueId).NotNull().WithMessage(v => $"{nameof(v.QueueId)} must have a value");
        }
    }

    public class AddTemplateToQueueCommandHandler : IRequestHandler<AddTemplateToQueueCommand, IEnumerable<DocumentTemplateDto>>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly IQueueRepository queueRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AddTemplateToQueueCommandHandler> logger;

        public AddTemplateToQueueCommandHandler(ITemplateRepository templateRepository
            , IQueueRepository queueRepository
            , IMapper mapper
            , ILogger<AddTemplateToQueueCommandHandler> logger)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DocumentTemplateDto>> Handle(AddTemplateToQueueCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetById(request.TemplateId, cancellationToken);
            var queue = await queueRepository.GetById(request.QueueId, cancellationToken);
            var queueTemplates = await templateRepository.FindByQueue(request.QueueId, cancellationToken);

            try
            {
                if (!queueTemplates.Any(t => t.Id == template.Id))
                {
                    await templateRepository.AddToQueue(template.Id, queue.Id, cancellationToken);
                    queueTemplates = await templateRepository.FindByQueue(queue.Id, cancellationToken);
                }

                return mapper.Map<IEnumerable<DocumentTemplateDto>>(queueTemplates);
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
