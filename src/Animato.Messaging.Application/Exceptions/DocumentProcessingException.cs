namespace Animato.Messaging.Application.Exceptions;

using Animato.Messaging.Domain.Entities;

public class DocumentProcessingException : DocumentException
{
    public DocumentProcessingException(DocumentId documentId, string message) : base(documentId, message) { }
    public DocumentProcessingException(DocumentId documentId, string message, Exception inner) : base(documentId, message, inner) { }
}
