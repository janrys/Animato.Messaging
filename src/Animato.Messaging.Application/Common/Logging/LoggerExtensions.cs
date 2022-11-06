namespace Animato.Messaging.Application.Common.Logging;

using MediatR;
using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static string ToLogString(this object item) => Newtonsoft.Json.JsonConvert.SerializeObject(item);

    public static void ApplicationEventSendDebug(this ILogger logger, INotification eventData)
        => logger.ApplicationEventSendDebug(eventData.GetType().Name, eventData.ToLogString());
}
