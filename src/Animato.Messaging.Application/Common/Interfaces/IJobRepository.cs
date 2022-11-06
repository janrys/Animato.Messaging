namespace Animato.Messaging.Application.Common.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Features.Documents.Contracts;
using Animato.Messaging.Domain.Entities;

public interface IJobRepository
{
    Task ReceiveDocument(InputDocument document, CancellationToken cancellationToken);
    Task ProcessDocument(ProcessedDocument document, CancellationToken cancellationToken);
    Task SendDocument(SendDocument document, CancellationToken cancellationToken);
    Task FailDocument(FailedDocument document, CancellationToken cancellationToken);
    Task<JobStatusDto> GetStatus(JobId jobId, CancellationToken cancellationToken);
    Task<DocumentStatusDto> GetStatus(DocumentId documentId, CancellationToken cancellationToken);
    Task<IEnumerable<JobId>> GetJobsToProcess(CancellationToken cancellationToken);
}
