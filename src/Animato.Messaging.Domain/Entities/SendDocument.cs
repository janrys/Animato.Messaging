namespace Animato.Messaging.Domain.Entities;
public class SendDocument : BaseDocument
{
    public TargetId TargetId { get; set; }
    public DateTime Processed { get; set; }
    public DateTime Send { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string FilePath { get; set; }

    public static SendDocument Create(InputDocument inputDocument)
        => new()
        {
            DocumentPriority = inputDocument.DocumentPriority,
            FilePath = "",
            Id = inputDocument.Id,
            JobId = inputDocument.JobId,
            ProcessorId = inputDocument.ProcessorId,
            QueueId = inputDocument.QueueId,
            QueuePriority = inputDocument.QueuePriority,
            Received = inputDocument.Received,
            ScheduleSendDate = inputDocument.ScheduleSendDate,
            TargetType = inputDocument.TargetType,
        };
}

