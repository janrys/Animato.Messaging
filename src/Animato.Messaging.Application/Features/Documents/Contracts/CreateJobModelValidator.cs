namespace Animato.Messaging.Application.Features.Queues.Contracts;

using Animato.Messaging.Application.Features.Documents.Contracts;
using FluentValidation;

public class CreateJobModelValidator : AbstractValidator<CreateJobModel>
{
    public CreateJobModelValidator()
    {
        RuleFor(v => v.QueueId).Must((m, id) => !string.IsNullOrEmpty(m.QueueId) || !string.IsNullOrEmpty(m.QueueName))
            .WithMessage(v => $"{nameof(v.QueueId)} or {nameof(v.QueueName)} must have a value");
        RuleFor(v => v.TemplateId).Must((m, tid) => !string.IsNullOrEmpty(m.TemplateId) || !string.IsNullOrEmpty(m.TemplateName))
            .WithMessage(v => $"{nameof(v.TemplateId)}  or {nameof(v.TemplateName)} must have a value");
        RuleFor(v => v.Priority).Must(p => !p.HasValue || p.Value >= 0)
            .WithMessage(v => $"{nameof(v.Priority)} must be greater or equal to 0 (or null)");
        RuleFor(v => v.Targets).NotEmpty().WithMessage(v => $"{nameof(v.Targets)} must have a value");
    }
}
