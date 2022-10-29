namespace Animato.Messaging.Application.Common.Interfaces;
using System.Collections.Generic;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public interface ITemplateProcessorFactory
{
    public IEnumerable<ITemplateProcessor> Processors { get; }
    public ITemplateProcessor GetProcessor(ProcessorId id);
    public ITemplateProcessor FindProcessor(ProcessorId id);
    public IEnumerable<ITemplateProcessor> GetProcessors(TargetType targetType);
}
