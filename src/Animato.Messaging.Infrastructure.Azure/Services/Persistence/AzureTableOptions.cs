namespace Animato.Messaging.Infrastructure.AzureStorage.Services.Persistence;
public class AzureTableStorageOptions
{
    public const string ConfigurationKey = nameof(AzureTableStorageOptions);
    public const string ArraySplitter = ",";

    public string ConnectionString { get; set; }
    public string QueuesTable { get; set; } = "queues";
    public string TemplatesTable { get; set; } = "templates";
}
