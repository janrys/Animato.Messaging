namespace Animato.Messaging.Application.Features.Documents;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Entities;
using FluentValidation;
using MediatR;

public class DocumentProcessedEvent : INotification
{
    public DocumentProcessedEvent(DocumentId documentId) => DocumentId = documentId;

    public DocumentId DocumentId { get; }

    public class DocumentProcessedEventValidator : AbstractValidator<DocumentProcessedEvent>
    {
        public DocumentProcessedEventValidator()
            => RuleFor(v => v.DocumentId).NotNull().WithMessage(v => $"{nameof(v.DocumentId)} must have a value");
    }

    public class DocumentProcessedHandler : INotificationHandler<DocumentProcessedEvent>
    {
        private readonly ISendDocumentService sendDocumentService;

        public DocumentProcessedHandler(ISendDocumentService sendDocumentService)
            => this.sendDocumentService = sendDocumentService ?? throw new ArgumentNullException(nameof(sendDocumentService));

        public Task Handle(DocumentProcessedEvent notification, CancellationToken cancellationToken)
            => sendDocumentService.Enqueue(notification.DocumentId, cancellationToken);
    }
}

