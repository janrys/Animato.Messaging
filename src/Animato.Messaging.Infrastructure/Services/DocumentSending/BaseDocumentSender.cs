namespace Animato.Messaging.Infrastructure.Services.DocumentSending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public class BaseDocumentSender : IDocumentSender
{
    private readonly TargetType[] targetTypes;

    public BaseDocumentSender() : this("79b58d3f-fc3a-4044-8412-82f24afc3dce", "")
    {

    }

    public BaseDocumentSender(string id, string name, params TargetType[] targetTypes)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        Id = new SenderId(Guid.Parse(id));
        Name = string.IsNullOrEmpty(name) ? GetType().Name : name;
        this.targetTypes = targetTypes ?? Array.Empty<TargetType>();
    }

    public SenderId Id { get; }
    public string Name { get; }
    public IEnumerable<TargetType> TargetTypes => Array.AsReadOnly(targetTypes);

    public bool CanSend(TargetType targetType) => targetTypes.Contains(targetType);
}
