namespace Animato.Messaging.Domain.Entities;
public class Queue
{
    public const int DefaultPriority = 5;
    public const int MinimumPriority = 1;

    public QueueId Id { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}
