namespace Animato.Messaging.Infrastructure.AzureStorage.Services.Persistence.DTOs;

using Animato.Messaging.Domain.Entities;
using Azure;
using Azure.Data.Tables;

public class QueueTableEntity : ITableEntity
{
    public QueueTableEntity()
    {

    }
    public QueueTableEntity(QueueId queueId, string name) : this(queueId.ToString(), name) { }
    public QueueTableEntity(string queueId, string name)
    {
        Id = queueId;
        Name = name;
    }

    public string Id { get => RowKey; set => RowKey = value; }
    public string Name { get => PartitionKey; private set => PartitionKey = value; }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public int Priority { get; set; }
    public bool IsActive { get; set; }

}


public static class ApplicationRoleTableEntityExtensions
{
    public static Queue ToEntity(this QueueTableEntity tableEntity)
     => new()
     {
         Id = new(Guid.Parse(tableEntity.Id)),
         Name = tableEntity.Name,
         IsActive = tableEntity.IsActive,
         Priority = tableEntity.Priority,
     };

    public static QueueTableEntity ToTableEntity(this Queue queue)
     => new(queue.Id, queue.Name)
     {
         IsActive = queue.IsActive,
         Priority = queue.Priority,
     };
}
