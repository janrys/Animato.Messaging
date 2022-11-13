namespace Animato.Messaging.Application.Common.Interfaces;
using System.Collections.Generic;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public interface IDocumentSenderFactory
{
    public IEnumerable<IDocumentSender> Senders { get; }
    public IDocumentSender GetSender(TargetType targetType, Target target);
    public IDocumentSender FindSender(TargetType targetType, Target target);
    public IEnumerable<IDocumentSender> GetSenders(TargetType targetType);
}
