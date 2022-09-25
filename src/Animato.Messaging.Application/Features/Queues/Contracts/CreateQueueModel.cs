namespace Animato.Messaging.Application.Features.Queues.Contracts;

using Animato.Messaging.Domain.Entities;

public class CreateQueueModel
{
    public string Name { get; set; }
    public int Priority { get; set; } = Queue.DefaultPriority;
    public bool IsActive { get; set; } = true;
}
