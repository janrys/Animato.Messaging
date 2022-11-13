namespace Animato.Messaging.Application.Common.Interfaces;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface ITemplateRepository
{
    Task<DocumentTemplate> GetById(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<Stream> GetContent(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<DocumentTemplate> Create(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task<DocumentTemplate> Update(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task Delete(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task Clear(CancellationToken cancellationToken);
    Task UpdateContent(DocumentTemplateId templateId, string fileName, Stream content, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentTemplate>> FindAll(CancellationToken cancellationToken);
    Task<DocumentTemplate> FindById(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentTemplate>> FindByQueue(QueueId queueId, CancellationToken cancellationToken);
    Task AddToQueue(DocumentTemplateId templateId, QueueId queueId, CancellationToken cancellationToken);
    Task RemoveFromQueue(DocumentTemplateId templateId, QueueId queueId, CancellationToken cancellationToken);
    Task<DocumentTemplate> GetByName(string templateName, CancellationToken cancellationToken);
    Task<DocumentTemplate> FindByName(string templateName, CancellationToken cancellationToken);
}
