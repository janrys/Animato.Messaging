namespace Animato.Messaging.Infrastructure.Services;

using Animato.Messaging.Application.Common.Interfaces;

public class StaticDateTimeService : IDateTimeService
{
    private readonly DateTime value;

    public StaticDateTimeService(DateTime value) => this.value = value;

    public DateTime Now => value.ToLocalTime();

    public DateTime UtcNow => value.ToUniversalTime();

    public DateTimeOffset UtcNowOffset => UtcNow;

    public DateTimeOffset NowOffset => Now;
}
