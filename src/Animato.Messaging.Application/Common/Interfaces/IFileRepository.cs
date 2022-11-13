namespace Animato.Messaging.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;

public interface IFileRepository
{
    Task<string> Save(string document, InputDocument inputDocument, CancellationToken cancellationToken);
    Task<string> GetFile(string path, CancellationToken cancellationToken);
}
