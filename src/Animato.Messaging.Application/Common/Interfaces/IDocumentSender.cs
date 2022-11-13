namespace Animato.Messaging.Application.Common.Interfaces;

using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public interface IDocumentSender
{
    SenderId Id { get; }

    public string Name { get; }
    public IEnumerable<TargetType> TargetTypes { get; }
    public bool CanSend(TargetType targetType);
}
