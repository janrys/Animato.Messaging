namespace Animato.Messaging.Application.Features.TemplateProcessors.Contracts;
using System.Collections.Generic;

public class RegisteredTemplateProcessor
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> TargetTypes { get; set; }
}
