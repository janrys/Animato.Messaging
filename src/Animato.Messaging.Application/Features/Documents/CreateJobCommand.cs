namespace Animato.Messaging.Application.Features.Documents;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateJobCommand : IRequest<JobDto>
{
    public CreateJobCommand(CreateJobModel job, ClaimsPrincipal user)
    {
        Job = job;
        User = user;
    }

    public CreateJobModel Job { get; }
    public ClaimsPrincipal User { get; }

    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(v => v.Job).NotNull().WithMessage(v => $"{nameof(v.Job)} must have a value");
            RuleFor(v => v.Job).SetValidator(new CreateJobModelValidator());
        }
    }

    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, JobDto>
    {
        private readonly ITemplateProcessorFactory templateProcessorFactory;
        private readonly IQueueRepository queueRepository;
        private readonly ITemplateRepository templateRepository;
        private readonly IJobRepository jobRepository;
        private readonly ITargetRepository targetRepository;
        private readonly IApplicationEventService applicationEventService;
        private readonly ILogger<CreateJobCommandHandler> logger;

        public CreateJobCommandHandler(ITemplateProcessorFactory templateProcessorFactory
            , IQueueRepository queueRepository
            , ITemplateRepository templateRepository
            , IJobRepository jobRepository
            , ITargetRepository targetRepository
            , IApplicationEventService applicationEventService
            , ILogger<CreateJobCommandHandler> logger)
        {
            this.templateProcessorFactory = templateProcessorFactory ?? throw new ArgumentNullException(nameof(templateProcessorFactory));
            this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            this.targetRepository = targetRepository ?? throw new ArgumentNullException(nameof(targetRepository));
            this.applicationEventService = applicationEventService ?? throw new ArgumentNullException(nameof(applicationEventService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<JobDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            DocumentTemplate template;

            if (!string.IsNullOrEmpty(request.Job.TemplateId))
            {
                DocumentTemplateId templateId;
                if (Guid.TryParse(request.Job.TemplateId, out var parsedTemplateId))
                {
                    templateId = new DocumentTemplateId(parsedTemplateId);
                }
                else
                {
                    throw new Exceptions.ValidationException(Exceptions.ValidationException.CreateFailure(nameof(request.Job.TemplateId), request.Job.TemplateId));
                }
                template = await templateRepository.GetById(templateId, cancellationToken);
            }
            else
            {
                template = await templateRepository.GetByName(request.Job.TemplateName, cancellationToken);
            }

            Queue queue;

            if (!string.IsNullOrEmpty(request.Job.QueueId))
            {
                QueueId queueId;
                if (Guid.TryParse(request.Job.QueueId, out var parsedQueueId))
                {
                    queueId = new QueueId(parsedQueueId);
                }
                else
                {
                    throw new Exceptions.ValidationException(Exceptions.ValidationException.CreateFailure(nameof(request.Job.TemplateId), request.Job.TemplateId));
                }

                queue = await queueRepository.GetById(queueId, cancellationToken);
            }
            else
            {
                queue = await queueRepository.GetByName(request.Job.QueueName, cancellationToken);
            }

            var processor = templateProcessorFactory.GetProcessor(template.ProcessorId);

            try
            {
                var targets = await targetRepository.CreateIfNotExists(request.Job.Targets.Select(s => new Target() { Address = s }), cancellationToken);
                var inputDocument = new InputDocument()
                {
                    JobId = JobId.New(),
                    TargetIds = new List<TargetId>(targets.Select(t => t.Id)),
                    TemplateId = template.Id,
                    ProcessorId = processor.Id,
                    Data = request.Job.Data.ToString(),
                    QueueId = queue.Id,
                    QueuePriority = queue.Priority,
                    DocumentPriority = request.Job.Priority ?? 0,
                    Received = DateTime.UtcNow,
                    ScheduleSendDate = request.Job.SendDate ?? DateTime.UtcNow,
                    TargetType = template.TargetType,
                };
                await jobRepository.ReceiveDocument(inputDocument, cancellationToken);
                await applicationEventService.Publish(new DocumentReceivedEvent(inputDocument.JobId), cancellationToken);

                return new JobDto() { Id = inputDocument.JobId.ToString() };
            }
            catch (BaseException) { throw; }
            catch (Exception exception)
            {
                logger.QueuesCreatingError(exception);
                throw new DataAccessException(LogMessageTexts.ErrorCreatingQueues, exception);
            }
        }
    }

}

