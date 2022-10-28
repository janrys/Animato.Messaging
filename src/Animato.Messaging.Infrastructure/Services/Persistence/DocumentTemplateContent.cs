namespace Animato.Messaging.Infrastructure.Services.Persistence;
using Animato.Messaging.Domain.Entities;

public class DocumentTemplateContent
{
    public DocumentTemplateId Id { get; set; }
    public string FileName { get; set; }
    public Stream Content { get; set; }

}
