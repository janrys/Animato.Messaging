namespace Animato.Messaging.Infrastructure.AzureStorage.Services;
using System;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class TableStorageHealthCheck : IHealthCheck
{
    public const string Name = "Azure table storage";
    private readonly IQueueRepository queueRepository;

    public TableStorageHealthCheck(IQueueRepository queueRepository) => this.queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        try
        {
            var scope = await queueRepository.FindAll(cancellationToken);
        }
        catch
        {
            isHealthy = false;
        }

        if (!isHealthy)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "Azure table infrastructure failes");
        }

        return HealthCheckResult.Healthy("Azure table infrastructure is ok");
    }
}
