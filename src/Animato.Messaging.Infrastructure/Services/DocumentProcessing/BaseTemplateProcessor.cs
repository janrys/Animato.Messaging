namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public class BaseTemplateProcessor : ITemplateProcessor
{
    private readonly TargetType[] targetTypes;
    public BaseTemplateProcessor() : this("a94429b4-65eb-4959-aa8a-1ef044162b63", "")
    {

    }

    public BaseTemplateProcessor(string id, string name, params TargetType[] targetTypes)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        Id = new ProcessorId(Guid.Parse(id));
        Name = string.IsNullOrEmpty(name) ? GetType().Name : name;
        this.targetTypes = targetTypes ?? Array.Empty<TargetType>();
    }

    public ProcessorId Id { get; }
    public IEnumerable<TargetType> TargetTypes => Array.AsReadOnly(targetTypes);

    public string Name { get; init; }

    public bool CanProcess(TargetType targetType) => targetTypes.Contains(targetType);
    public virtual Task<string> Generate(Stream templateStream, object data, TargetType targetType, CancellationToken cancellationToken)
        => Task.FromResult(DateTime.UtcNow.ToString(GlobalOptions.DatePattern, GlobalOptions.Culture));
}
