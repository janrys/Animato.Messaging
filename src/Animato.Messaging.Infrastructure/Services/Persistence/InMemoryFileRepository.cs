namespace Animato.Messaging.Infrastructure.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using Microsoft.Extensions.Logging;

public class InMemoryFileRepository : IFileRepository
{
    private readonly Dictionary<string, string> files;
    private readonly ILogger<InMemoryFileRepository> logger;

    public InMemoryFileRepository(InMemoryDataContext dataContext, ILogger<InMemoryFileRepository> logger)
    {
        if (dataContext is null)
        {
            throw new ArgumentNullException(nameof(dataContext));
        }

        files = dataContext.Files;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<string> Save(string document, InputDocument inputDocument, CancellationToken cancellationToken)
    {
        var path = GetPath(inputDocument.Id, inputDocument.TargetType);

        try
        {
            files.Add(path, document);
            return Task.FromResult(path);
        }
        catch (Exception exception)
        {
            logger.DocumentsUpdatingError(exception);
            throw;
        }
    }

    private string GetPath(DocumentId id, TargetType targetType)
        => $"{targetType.Name}/{id}";

    public Task<string> GetFile(string path, CancellationToken cancellationToken)
    {
        try
        {
            if (!files.TryGetValue(path, out var file) || file is null)
            {
                throw new NotFoundException(nameof(ProcessedDocument.FilePath), path);
            }

            return Task.FromResult(file);
        }
        catch (Exception exception)
        {
            logger.DocumentsLoadingError(exception);
            throw;
        }
    }
}
