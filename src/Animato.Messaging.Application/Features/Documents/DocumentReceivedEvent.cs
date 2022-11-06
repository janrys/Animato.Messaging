namespace Animato.Messaging.Application.Features.Documents;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Features.Documents.Contracts;
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
        public DocumentReceivedHandler()
        {
        }

        public Task Handle(DocumentReceivedEvent notification, CancellationToken cancellationToken)
        {

        }
    }
}

