namespace Animato.Messaging.Application.Exceptions;

using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Exceptions;

public class DocumentException : BaseException
{
    public DocumentException(DocumentId documentId, string message) : base(message) { }
    public DocumentException(DocumentId documentId, string message, Exception inner) : base(message, inner) { }
}
