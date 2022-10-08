namespace Animato.Messaging.Application.Common.Logging;
using Microsoft.Extensions.Logging;


public static partial class LogMessages
{
    /* TRACE >= 0  */

    /* DEBUG >= 5000  */

    /* INFORMATION >= 10000  */
    [LoggerMessage(10001, LogLevel.Information, "Using {LayerName} persistence layer")]
    public static partial void PersistenceLayerLoadingInformation(this ILogger logger, string layerName);

    [LoggerMessage(10002, LogLevel.Information, "{ApplicationName} seeded with id {ApplicationId} and client id {ClientId}")]
    public static partial void SsoSeededInformation(this ILogger logger, string applicationName, string applicationId, string clientId);

    [LoggerMessage(10003, LogLevel.Information, "SSO admin seeded. Change password ASAP. Login {Login}, id {Id}, password {Password}, TOTP secret key {TotpSecretKey}")]
    public static partial void SsoAdminSeededInformation(this ILogger logger, string login, string id, string password, string totpSecretKey);

    [LoggerMessage(10004, LogLevel.Information, "{DomainName} seeded")]
    public static partial void DataSeededInformation(this ILogger logger, string domainName);

    [LoggerMessage(10005, LogLevel.Information, "{ServiceName} starting")]
    public static partial void ServiceStartingInformation(this ILogger logger, string serviceName);

    [LoggerMessage(10006, LogLevel.Information, "{ServiceName} finished")]
    public static partial void ServiceFinishedInformation(this ILogger logger, string serviceName);

    /* WARNINGS >= 15000  */

    /* ERRORS >= 20000  */

    [LoggerMessage(20001, LogLevel.Error, LogMessageTexts.ErrorLoadingQueues)]
    public static partial void QueuesLoadingError(this ILogger logger, Exception exception);

    [LoggerMessage(20002, LogLevel.Error, LogMessageTexts.ErrorCreatingQueues)]
    public static partial void QueuesCreatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20003, LogLevel.Error, LogMessageTexts.ErrorUpdatingQueues)]
    public static partial void QueuesUpdatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20004, LogLevel.Error, LogMessageTexts.ErrorDeletingQueues)]
    public static partial void QueuesDeletingError(this ILogger logger, Exception exception);

    [LoggerMessage(20005, LogLevel.Error, LogMessageTexts.ErrorLoadingTemplates)]
    public static partial void TemplatesLoadingError(this ILogger logger, Exception exception);

    [LoggerMessage(20006, LogLevel.Error, LogMessageTexts.ErrorCreatingTemplates)]
    public static partial void TemplatesCreatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20007, LogLevel.Error, LogMessageTexts.ErrorUpdatingTemplates)]
    public static partial void TemplatesUpdatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20008, LogLevel.Error, LogMessageTexts.ErrorDeletingTemplates)]
    public static partial void TemplatesDeletingError(this ILogger logger, Exception exception);

    /* CRITICAL >= 30000  */


}
