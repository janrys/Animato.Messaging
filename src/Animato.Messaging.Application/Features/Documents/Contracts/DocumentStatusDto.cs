namespace Animato.Messaging.Application.Features.Documents.Contracts;

using Animato.Messaging.Domain.Enums;

public class DocumentStatusDto
{
    public string Id { get; set; }
    public DocumentStatus Status { get; set; }
}
