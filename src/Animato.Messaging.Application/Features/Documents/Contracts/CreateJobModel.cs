namespace Animato.Messaging.Application.Features.Documents.Contracts;
using System;

public class CreateJobModel
{
    public string QueueId { get; set; }
    public string QueueName { get; set; }
    public string TemplateId { get; set; }
    public string TemplateName { get; set; }
    public DateTime? SendDate { get; set; }
    public int? Priority { get; set; }
    public List<string> Targets { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
