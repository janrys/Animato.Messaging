namespace Animato.Messaging.Application.Features.Queues.Contracts;

using Animato.Messaging.Application.Features.Documents.Contracts;
using FluentValidation;

public class CreateJobModelValidator : AbstractValidator<CreateJobModel>
{
    public CreateJobModelValidator()
    {
        RuleFor(v => v.QueueId).NotEmpty().WithMessage(v => $"{nameof(v.QueueId)} must have a value");
        RuleFor(v => v.TemplateId).NotEmpty().WithMessage(v => $"{nameof(v.TemplateId)} must have a value");
        RuleFor(v => v.Priority).Must(p => !p.HasValue || p.Value >= 0)
            .WithMessage(v => $"{nameof(v.Priority)} must be greater or equal to 0 (or null)");
        RuleFor(v => v.Targets).NotEmpty().WithMessage(v => $"{nameof(v.Targets)} must have a value");
    }
}
