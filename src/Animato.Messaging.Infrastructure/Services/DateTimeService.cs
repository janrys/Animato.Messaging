namespace Animato.Messaging.Infrastructure.Services;

using Animato.Messaging.Application.Common.Interfaces;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    public DateTimeOffset NowOffset => DateTimeOffset.Now;
}
