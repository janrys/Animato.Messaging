namespace Animato.Messaging.Application.Features.Queues.Contracts;
public class QueueDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}
