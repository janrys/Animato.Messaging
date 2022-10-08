namespace Animato.Messaging.Application.Features.Templates.Contracts;

using Animato.Messaging.Domain.Enums;

public class CreateDocumentTemplateModel
{
    public string Name { get; set; }
    public string ProcessorId { get; set; }
    public TargetType TargetType { get; set; }
}
