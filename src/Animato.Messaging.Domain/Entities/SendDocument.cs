namespace Animato.Messaging.Domain.Entities;
public class SendDocument : BaseDocument
{
    public TargetId TargetId { get; set; }
    public DateTime Processed { get; set; }
    public DateTime Send { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string FilePath { get; set; }

    public static SendDocument Create(ProcessedDocument processedDocument)
        => new()
        {
            DocumentPriority = processedDocument.DocumentPriority,
            FilePath = "",
            Id = processedDocument.Id,
            JobId = processedDocument.JobId,
            ProcessorId = processedDocument.ProcessorId,
            QueueId = processedDocument.QueueId,
            QueuePriority = processedDocument.QueuePriority,
            Received = processedDocument.Received,
            ScheduleSendDate = processedDocument.ScheduleSendDate,
            TargetType = processedDocument.TargetType,
            Processed = processedDocument.Processed,
            TargetId = processedDocument.TargetId,
        };
}

