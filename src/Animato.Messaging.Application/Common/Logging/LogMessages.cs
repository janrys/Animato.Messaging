namespace Animato.Messaging.Application.Common.Logging;

using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;


public static partial class LogMessages
{
    /* TRACE >= 0  */

    /* DEBUG >= 5000  */
    [LoggerMessage(5001, LogLevel.Debug, "Sending application event {EventType}: {EventData}")]
    public static partial void ApplicationEventSendDebug(this ILogger logger, string eventType, string eventData);

    [LoggerMessage(5002, LogLevel.Debug, "Executed document processing, Found {DocumentCount} to process")]
    public static partial void DocumentProcessingDebug(this ILogger logger, int documentCount);

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

    [LoggerMessage(10007, LogLevel.Information, "Starting processing job {JobId}")]
    public static partial void StartProcessingDocumentInformation(this ILogger logger, JobId jobId);

    [LoggerMessage(10008, LogLevel.Information, "Finished processing job {JobId} succesfully")]
    public static partial void FinishedProcessingDocumentInformation(this ILogger logger, JobId jobId);

    [LoggerMessage(10009, LogLevel.Information, "Starting sending document {DocumentId}")]
    public static partial void StartSendingDocumentInformation(this ILogger logger, DocumentId documentId);

    [LoggerMessage(10010, LogLevel.Information, "Finished sending document {DocumentId}, job {JobId} succesfully")]
    public static partial void FinishedSendingDocumentInformation(this ILogger logger, DocumentId documentId, JobId jobId);

    [LoggerMessage(10011, LogLevel.Information, "Sending document to {Address}, content {file}")]
    public static partial void SendingDocumentInformation(this ILogger logger, string address, string file);

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

    [LoggerMessage(20009, LogLevel.Error, "Error generating document: {ProcessorError}")]
    public static partial void TemplateProcessorError(this ILogger logger, string processorError);
    [LoggerMessage(20010, LogLevel.Error, LogMessageTexts.ErrorLoadingTargets)]
    public static partial void TargetsLoadingError(this ILogger logger, Exception exception);

    [LoggerMessage(20011, LogLevel.Error, LogMessageTexts.ErrorCreatingTargets)]
    public static partial void TargetsCreatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20012, LogLevel.Error, LogMessageTexts.ErrorUpdatingTargets)]
    public static partial void TargetsUpdatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20013, LogLevel.Error, LogMessageTexts.ErrorDeletingTargets)]
    public static partial void TargetsDeletingError(this ILogger logger, Exception exception);
    [LoggerMessage(20014, LogLevel.Error, LogMessageTexts.ErrorLoadingDocuments)]
    public static partial void DocumentsLoadingError(this ILogger logger, Exception exception);

    [LoggerMessage(20015, LogLevel.Error, LogMessageTexts.ErrorCreatingDocuments)]
    public static partial void DocumentsCreatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20016, LogLevel.Error, LogMessageTexts.ErrorUpdatingDocuments)]
    public static partial void DocumentsUpdatingError(this ILogger logger, Exception exception);

    [LoggerMessage(20017, LogLevel.Error, LogMessageTexts.ErrorDeletingDocuments)]
    public static partial void DocumentsDeletingError(this ILogger logger, Exception exception);

    [LoggerMessage(20018, LogLevel.Error, "Document processing failed. Job {JobId}")]
    public static partial void FinishedProcessingDocumentError(this ILogger logger, JobId jobId, Exception exception);
    [LoggerMessage(20019, LogLevel.Error, "Document {DocumentId} sending failed. Job {JobId}")]
    public static partial void FinishedSendingDocumentError(this ILogger logger, DocumentId documentId, JobId jobId, Exception exception);

    /* CRITICAL >= 30000  */


}
