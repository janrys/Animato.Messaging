namespace Animato.Messaging.Application.Common.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;

public interface IJobRepository
{
    Task ReceiveDocument(InputDocument document, CancellationToken cancellationToken);
    Task ProcessDocument(ProcessedDocument document, CancellationToken cancellationToken);
    Task SendDocument(SendDocument document, CancellationToken cancellationToken);
    Task FailDocument(FailedDocument document, CancellationToken cancellationToken);
    Task<JobStatusDto> GetStatus(JobId jobId, CancellationToken cancellationToken);
    Task<DocumentStatusDto> GetStatus(DocumentId documentId, CancellationToken cancellationToken);
    Task<IEnumerable<JobId>> GetJobsToProcess(CancellationToken cancellationToken);
    Task<IEnumerable<DocumentId>> GetDocumentsToSend(CancellationToken cancellationToken);
    Task<InputDocument> StartProcessingJob(JobId jobId, CancellationToken cancellationToken);
    Task<ProcessedDocument> StartSendingDocument(DocumentId documentId, CancellationToken cancellationToken);
    Task RemoveProcessingJob(JobId jobId, CancellationToken cancellationToken);
    Task<IEnumerable<JobStatusDto>> GetStatus(QueueType queueType, CancellationToken cancellationToken);
}
