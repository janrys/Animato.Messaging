namespace Animato.Messaging.Application.Exceptions;

using Animato.Messaging.Domain.Exceptions;

public class DataAccessException : BaseException
{
    public DataAccessException(string message) : base(message) { }
    public DataAccessException(string message, Exception inner) : base(message, inner) { }
}
