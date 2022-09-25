namespace Animato.Messaging.Domain.Entities;


public class SendDocument : BaseDocument
{
    public TargetId TargetId { get; set; }
    public DateTime Processed { get; set; }
    public DateTime Send { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string FilePath { get; set; }
}

