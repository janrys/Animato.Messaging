namespace Animato.Messaging.Infrastructure.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;
using Microsoft.Extensions.Logging;

public class InMemoryTargetRepository : ITargetRepository
{
    private readonly List<Target> targets;
    private readonly ILogger<InMemoryTargetRepository> logger;

    public InMemoryTargetRepository(InMemoryDataContext dataContext, ILogger<InMemoryTargetRepository> logger)
    {
        if (dataContext is null)
        {
            throw new ArgumentNullException(nameof(dataContext));
        }

        targets = dataContext.Targets;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<IEnumerable<Target>> FindAll(CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(targets.AsEnumerable());
        }
        catch (Exception exception)
        {
            logger.TargetsLoadingError(exception);
            throw;
        }
    }

    public async Task<Target> GetById(TargetId targetId, CancellationToken cancellationToken)
    {
        var template = await FindById(targetId, cancellationToken);

        if (template is null)
        {
            throw new NotFoundException(nameof(Target), targetId);
        }

        return template;
    }

    public Task<IEnumerable<Target>> FindById(IEnumerable<TargetId> targetIds, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(targets.Join(targetIds, t => t.Id, i => i, (t, i) => t));
        }
        catch (Exception exception)
        {
            logger.TargetsLoadingError(exception);
            throw;
        }
    }


    public Task<Target> FindById(TargetId targetId, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(targets.FirstOrDefault(u => u.Id == targetId));
        }
        catch (Exception exception)
        {
            logger.TargetsLoadingError(exception);
            throw;
        }
    }

    public Task<Target> Create(Target target, CancellationToken cancellationToken)
    {
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (target.Id == TargetId.Empty)
        {
            target.Id = TargetId.New();
        }

        try
        {
            targets.Add(target);
            return Task.FromResult(target);
        }
        catch (Exception exception)
        {
            logger.TargetsCreatingError(exception);
            throw;
        }
    }

    public async Task<Target> CreateIfNotExists(Target target, CancellationToken cancellationToken)
    {
        var storedTarget = await FindById(target.Id, cancellationToken);
        storedTarget ??= await Create(target, cancellationToken);
        return storedTarget;
    }

    public async Task<IEnumerable<Target>> CreateIfNotExists(IEnumerable<Target> targets, CancellationToken cancellationToken)
    {
        var results = new List<Target>();
        foreach (var target in targets)
        {
            results.Add(await CreateIfNotExists(target, cancellationToken));
        }
        return results;
    }

    public Task Delete(TargetId targetId, CancellationToken cancellationToken)
    {
        try
        {
            return Task.FromResult(targets.RemoveAll(c => c.Id == targetId));
        }
        catch (Exception exception)
        {
            logger.TargetsDeletingError(exception);
            throw;
        }
    }

    public Task Clear(CancellationToken cancellationToken)
    {
        try
        {
            targets.Clear();
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            logger.TargetsDeletingError(exception);
            throw;
        }
    }

}
