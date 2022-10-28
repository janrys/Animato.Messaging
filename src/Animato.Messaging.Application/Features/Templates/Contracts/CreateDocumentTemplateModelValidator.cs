namespace Animato.Messaging.Application.Features.Templates.Contracts;

using Animato.Messaging.Domain.Enums;
using FluentValidation;

public class CreateDocumentTemplateModelValidator : AbstractValidator<CreateDocumentTemplateModel>
{
    public CreateDocumentTemplateModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage(v => $"{nameof(v.Name)} must have a value");
        RuleFor(v => v.ProcessorId).NotEmpty().WithMessage(v => $"{nameof(v.ProcessorId)} must have a value");
        RuleFor(v => v.ProcessorId).Must(p => Guid.TryParse(p, out _))
            .WithMessage(v => $"{nameof(v.ProcessorId)} has a wrong value {v.ProcessorId}");
        RuleFor(v => v.TargetType).NotEmpty().WithMessage(v => $"{nameof(v.TargetType)} must have a value");
        RuleFor(v => v.TargetType).Must(t => TargetType.TryFromName(t, true, out _))
            .WithMessage(v => $"{nameof(v.TargetType)} has a wrong value {v.TargetType}");
    }
}
