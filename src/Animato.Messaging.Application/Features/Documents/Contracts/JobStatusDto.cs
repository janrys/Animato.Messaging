namespace Animato.Messaging.Application.Features.Documents.Contracts;

public class JobStatusDto
{
    public string Id { get; set; }
    public List<DocumentStatusDto> Documents { get; set; } = new List<DocumentStatusDto>();
    public int Count => Documents.Count;
    public int Failed => Documents.Count(d => d.Status == Domain.Enums.DocumentStatus.Failed);
    public int Send => Documents.Count(d => d.Status == Domain.Enums.DocumentStatus.Send);
    public int ToProcess => Documents.Count(d => d.Status == Domain.Enums.DocumentStatus.WaitingToProcess
        || d.Status == Domain.Enums.DocumentStatus.BeingProcessed);
    public int ToSend => Documents.Count(d => d.Status == Domain.Enums.DocumentStatus.WaitingToSend
        || d.Status == Domain.Enums.DocumentStatus.BeingSend
        || d.Status == Domain.Enums.DocumentStatus.WaitingForSchedule);
}
