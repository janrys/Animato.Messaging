namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;

using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Enums;
using Microsoft.Extensions.Logging;

public class LongRunningDebugTemplateProcessor : DebugTemplateProcessor
{
    public new const string ProcessorId = "a94429b4-2222-2222-aa8a-1ef044162b63";
    private readonly ILogger<LongRunningDebugTemplateProcessor> logger;
    private readonly int delayInMs;
    private readonly int failPercentage;

    public LongRunningDebugTemplateProcessor(ILogger<LongRunningDebugTemplateProcessor> logger, int delayInMs = 10000, int failPercentage = 10)
        : base(ProcessorId, "", logger, TargetType.List.ToArray())
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.delayInMs = delayInMs;
        this.failPercentage = (failPercentage is < 0 or > 100) ? 100 : failPercentage;
    }

    public override async Task<string> Generate(Stream templateStream, object data, TargetType targetType, CancellationToken cancellationToken)
    {
        await Task.Delay(delayInMs, cancellationToken);

        if (failPercentage != 0 && RandomNumberGenerator.GetInt32(0, 100) <= failPercentage)
        {
            throw new DocumentProcessorException($"Test processing document failure from {nameof(LongRunningDebugTemplateProcessor)}");
        }

        return await base.Generate(templateStream, data, targetType, cancellationToken);
    }
}

