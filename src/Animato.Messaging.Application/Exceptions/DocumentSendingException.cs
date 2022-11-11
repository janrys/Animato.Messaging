namespace Animato.Messaging.Application.Exceptions;

using Animato.Messaging.Domain.Entities;

public class DocumentSendingException : DocumentException
{
    public DocumentSendingException(DocumentId documentId, string message) : base(documentId, message) { }
    public DocumentSendingException(DocumentId documentId, string message, Exception inner) : base(documentId, message, inner) { }
}
