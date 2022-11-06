namespace Animato.Messaging.Application.Common.Interfaces;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface IProcessDocumentService
{
    Task CheckWaiting(CancellationToken cancellationToken);
    Task Enqueue(JobId jobId, CancellationToken cancellationToken);
}
