namespace Animato.Messaging.Domain.Entities;


public class ProcessedDocument : BaseDocument
{
    public TargetId TargetId { get; set; }
    public DateTime Processed { get; set; }
    public ProcessorId ProcessorId { get; set; }
    public string FilePath { get; set; }
    public DateTime? SendingStarted { get; set; }

    public static ProcessedDocument Create(InputDocument inputDocument)
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

