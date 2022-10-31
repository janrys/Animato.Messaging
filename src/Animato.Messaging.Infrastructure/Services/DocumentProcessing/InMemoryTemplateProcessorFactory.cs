namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public class InMemoryTemplateProcessorFactory : ITemplateProcessorFactory
{
    public InMemoryTemplateProcessorFactory(IEnumerable<ITemplateProcessor> templateProcessors)
    {
        Processors = templateProcessors ?? throw new ArgumentNullException(nameof(templateProcessors));

        if (Processors.GroupBy(p => p.Id).Any(g => g.Count() > 1))
        {
            throw new ValidationException(ValidationException.CreateFailure(nameof(ITemplateProcessor.Id), "Duplicit template processor ids"));
        }
    }
    public IEnumerable<ITemplateProcessor> Processors { get; }
    public ITemplateProcessor GetProcessor(ProcessorId id)
        => Processors.SingleOrDefault(p => p.Id == id) ?? throw new NotFoundException(nameof(ITemplateProcessor), id);
    public ITemplateProcessor FindProcessor(ProcessorId id) => Processors.SingleOrDefault(p => p.Id == id);
    public IEnumerable<ITemplateProcessor> GetProcessors(TargetType targetType) => Processors.Where(p => p.TargetTypes.Contains(targetType));
}


