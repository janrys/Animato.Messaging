namespace Animato.Messaging.Application.Common.Interfaces;
using System.Collections.Generic;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public interface ITemplateProcessor
{
    public ProcessorId Id { get; }
    public string Name { get; }
    public IEnumerable<TargetType> TargetTypes { get; }
    public bool CanProcess(TargetType targetType);
}
