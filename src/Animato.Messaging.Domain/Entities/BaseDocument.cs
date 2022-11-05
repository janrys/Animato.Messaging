namespace Animato.Messaging.Domain.Entities;

using Animato.Messaging.Domain.Enums;

public abstract class BaseDocument
{
    public DocumentId Id { get; set; }
    public JobId JobId { get; set; }
    public QueueId QueueId { get; set; }
    public int QueuePriority { get; set; }
    public int DocumentPriority { get; set; }
    public DateTime ScheduleSendDate { get; set; }
    public DateTime Received { get; set; }
    public TargetType TargetType { get; set; }

}

