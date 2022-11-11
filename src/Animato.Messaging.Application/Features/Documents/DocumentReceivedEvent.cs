namespace Animato.Messaging.Application.Features.Documents;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Entities;
using FluentValidation;
using MediatR;

public class DocumentReceivedEvent : INotification
{
    public DocumentReceivedEvent(JobId jobId) => JobId = jobId;

    public JobId JobId { get; }

    public class DocumentReceivedEventValidator : AbstractValidator<DocumentReceivedEvent>
    {
        public DocumentReceivedEventValidator()
            => RuleFor(v => v.JobId).NotNull().WithMessage(v => $"{nameof(v.JobId)} must have a value");
    }

    public class DocumentReceivedHandler : INotificationHandler<DocumentReceivedEvent>
    {
        private readonly IProcessDocumentService processDocumentService;

        public DocumentReceivedHandler(IProcessDocumentService processDocumentService)
            => this.processDocumentService = processDocumentService ?? throw new ArgumentNullException(nameof(processDocumentService));

        public Task Handle(DocumentReceivedEvent notification, CancellationToken cancellationToken)
            => processDocumentService.Enqueue(notification.JobId, cancellationToken);
    }
}

