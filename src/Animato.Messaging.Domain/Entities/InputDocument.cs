namespace Animato.Messaging.Domain.Entities;


public class InputDocument : BaseDocument
{
    public List<TargetId> TargetIds { get; set; }
    public DocumentTemplateId TemplateId { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string Data { get; set; }
    public DateTime? ProcessingStarted { get; set; }
}

