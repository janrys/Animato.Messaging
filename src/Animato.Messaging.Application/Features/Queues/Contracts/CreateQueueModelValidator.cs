namespace Animato.Messaging.Application.Features.Queues.Contracts;

using Animato.Messaging.Domain.Entities;
using FluentValidation;

public class CreateQueueModelValidator : AbstractValidator<CreateQueueModel>
{
    public CreateQueueModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage(v => $"{nameof(v.Name)} must have a value");
        RuleFor(v => v.Priority).GreaterThanOrEqualTo(Queue.MinimumPriority)
            .WithMessage(v => $"{nameof(v.Priority)} must greater or equal to {Queue.MinimumPriority}");
    }
}
