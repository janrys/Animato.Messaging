namespace Animato.Messaging.Application.Common.Interfaces;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface ISendDocumentService
{
    Task CheckWaiting(CancellationToken cancellationToken);
    Task Enqueue(DocumentId documentId, CancellationToken cancellationToken)
    Task Send(CancellationToken cancellationToken);
}
