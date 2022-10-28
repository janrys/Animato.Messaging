namespace Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public class DocumentTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public TargetType TargetType { get; set; }
}
