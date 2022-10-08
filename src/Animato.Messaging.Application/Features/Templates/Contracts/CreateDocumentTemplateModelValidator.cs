namespace Animato.Messaging.Application.Features.Templates.Contracts;

using FluentValidation;

public class CreateDocumentTemplateModelValidator : AbstractValidator<CreateDocumentTemplateModel>
{
    public CreateDocumentTemplateModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage(v => $"{nameof(v.Name)} must have a value");
        RuleFor(v => v.ProcessorId).NotEmpty().WithMessage(v => $"{nameof(v.ProcessorId)} must have a value");
        RuleFor(v => v.TargetType).NotEmpty().WithMessage(v => $"{nameof(v.TargetType)} must have a value");
    }
}
