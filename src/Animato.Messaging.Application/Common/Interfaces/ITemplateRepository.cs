namespace Animato.Messaging.Application.Common.Interfaces;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface ITemplateRepository
{
    Task<IEnumerable<DocumentTemplate>> FindAll(CancellationToken cancellationToken);
    Task<DocumentTemplate> FindById(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<DocumentTemplate> GetById(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<DocumentTemplate> Create(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task<DocumentTemplate> Update(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task Delete(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task Clear(CancellationToken cancellationToken);
    Task UpdateContent(DocumentTemplateId templateId, string fileName, Stream content, CancellationToken cancellationToken);
}
