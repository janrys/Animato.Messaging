namespace Animato.Messaging.Application.Exceptions;

using Animato.Messaging.Domain.Exceptions;

public class ForbiddenAccessException : BaseException
{
    public ForbiddenAccessException(string userName, string action) : this($"Action {action} is not allowed for user {userName}") { }
    public ForbiddenAccessException(string message) : base(message) { }
}
