namespace Animato.Messaging.Application.Common.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface ITargetRepository
{
    Task<Target> GetById(TargetId targetId, CancellationToken cancellationToken);
    Task<Target> Create(Target target, CancellationToken cancellationToken);
    Task<Target> CreateIfNotExists(Target target, CancellationToken cancellationToken);
    Task<IEnumerable<Target>> CreateIfNotExists(IEnumerable<Target> targets, CancellationToken cancellationToken);
    Task Delete(TargetId targetId, CancellationToken cancellationToken);
    Task Clear(CancellationToken cancellationToken);
    Task<IEnumerable<Target>> FindAll(CancellationToken cancellationToken);
    Task<Target> FindById(TargetId targetId, CancellationToken cancellationToken);
    Task<IEnumerable<Target>> FindById(IEnumerable<TargetId> targetIds, CancellationToken cancellationToken);
}
