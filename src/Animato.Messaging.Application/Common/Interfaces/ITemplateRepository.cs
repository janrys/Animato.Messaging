namespace Animato.Messaging.Application.Common.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface ITemplateRepository
{
    Task<IEnumerable<DocumentTemplate>> GetAll(CancellationToken cancellationToken);
    Task<DocumentTemplate> GetById(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task<DocumentTemplate> Create(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task<DocumentTemplate> Update(DocumentTemplate documentTemplate, CancellationToken cancellationToken);
    Task Delete(DocumentTemplateId templateId, CancellationToken cancellationToken);
    Task Clear(CancellationToken cancellationToken);
}
