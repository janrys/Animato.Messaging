namespace Animato.Messaging.Domain.Entities;

using Animato.Messaging.Domain.Enums;

public class DocumentTemplate
{
    public DocumentTemplateId Id { get; set; }
    public string Name { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public TargetType TargetType { get; set; }

}
