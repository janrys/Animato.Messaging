namespace Animato.Messaging.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTimeOffset UtcNowOffset { get; }
}
