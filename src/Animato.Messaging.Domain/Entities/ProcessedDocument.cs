namespace Animato.Messaging.Domain.Entities;


public class ProcessedDocument : BaseDocument
{
    public TargetId TargetId { get; set; }
    public DateTime Processed { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string FilePath { get; set; }
    public DateTime? SendingStarted { get; set; }
}

