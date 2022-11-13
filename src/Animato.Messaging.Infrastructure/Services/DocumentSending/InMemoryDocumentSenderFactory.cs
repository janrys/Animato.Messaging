namespace Animato.Messaging.Infrastructure.Services.DocumentSending;
using System;
using System.Collections.Generic;
using System.Linq;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public class InMemoryDocumentSenderFactory : IDocumentSenderFactory
{
    public InMemoryDocumentSenderFactory(IEnumerable<IDocumentSender> senders)
    {
        Senders = senders ?? throw new ArgumentNullException(nameof(senders));

        if (Senders.GroupBy(p => p.Id).Any(g => g.Count() > 1))
        {
            throw new ValidationException(ValidationException.CreateFailure(nameof(ITemplateProcessor.Id), "Duplicit sender ids"));
        }
    }
    public IEnumerable<IDocumentSender> Senders { get; }
    public IDocumentSender GetSender(SenderId id)
        => Senders.SingleOrDefault(p => p.Id == id) ?? throw new NotFoundException(nameof(IDocumentSender), id);
    public IDocumentSender FindSender(SenderId id) => Senders.SingleOrDefault(p => p.Id == id);
    public IEnumerable<IDocumentSender> GetSenders(TargetType targetType) => Senders.Where(p => p.TargetTypes.Contains(targetType));
    public IDocumentSender GetSender(TargetType targetType, Target target)
        => FindSender(targetType, target) ?? throw new NotFoundException(nameof(IDocumentSender), targetType.Name);
    public IDocumentSender FindSender(TargetType targetType, Target target)
        => Senders.FirstOrDefault(s => s.CanSend(targetType));
}


