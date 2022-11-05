namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;

using System;
using System.Runtime.Serialization;
using Animato.Messaging.Domain.Exceptions;

[Serializable]
internal class DocumentProcessorException : BaseException
{
    public DocumentProcessorException()
    {
    }

    public DocumentProcessorException(string message) : base(message)
    {
    }

    public DocumentProcessorException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected DocumentProcessorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
