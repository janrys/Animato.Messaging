namespace Animato.Messaging.Domain.Entities;

public class FailedDocument : BaseDocument
{
    public BaseDocument Document { get; set; }
    public DateTime Failed { get; set; }
    public List<string> Errors { get; set; }
}

