namespace Animato.Messaging.Infrastructure.Services.DocumentSending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using Microsoft.Extensions.Logging;

public class BaseDocumentSender : IDocumentSender
{
    public const string SenderId = "79b58d3f-fc3a-4044-8412-82f24afc3dce";
    private readonly ILogger<BaseDocumentSender> logger;
    private readonly TargetType[] targetTypes;

    public BaseDocumentSender(ILogger<BaseDocumentSender> logger) : this(SenderId, "", logger)
    {

    }

    public BaseDocumentSender(string id, string name, ILogger<BaseDocumentSender> logger, params TargetType[] targetTypes)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        Id = new SenderId(Guid.Parse(id));
        Name = string.IsNullOrEmpty(name) ? GetType().Name : name;
        this.logger = logger;
        this.targetTypes = targetTypes ?? Array.Empty<TargetType>();
    }

    public SenderId Id { get; }
    public string Name { get; }
    public IEnumerable<TargetType> TargetTypes => Array.AsReadOnly(targetTypes);

    public bool CanSend(TargetType targetType) => targetTypes.Contains(targetType);
    public virtual Task Send(string file, Target target, CancellationToken cancellationToken)
    {
        logger.SendingDocumentInformation(target.Address, file[..Math.Min(file.Length, 100)]);
        return Task.CompletedTask;
    }
}
