namespace Animato.Messaging.Application.Features.Documents.Contracts;
using System;
using Newtonsoft.Json.Linq;

public class CreateJobModel
{
    public string QueueId { get; set; }
    public string TemplateId { get; set; }
    public DateTime? SendDate { get; set; }
    public int? Priority { get; set; }
    public List<string> Targets { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
