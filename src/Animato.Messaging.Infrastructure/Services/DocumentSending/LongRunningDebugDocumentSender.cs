namespace Animato.Messaging.Infrastructure.Services.DocumentSending;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using Animato.Messaging.Infrastructure.Services.DocumentProcessing;
using Microsoft.Extensions.Logging;

public class LongRunningDebugDocumentSender : BaseDocumentSender
{
    public new const string SenderId = "79b58d3f-3333-4444-8412-82f24afc3dce";

    private readonly ILogger<LongRunningDebugDocumentSender> logger;
    private readonly int delayInMs;
    private readonly int failPercentage;

    public LongRunningDebugDocumentSender(ILogger<LongRunningDebugDocumentSender> logger, int delayInMs = 10000, int failPercentage = 10)
        : base(SenderId, nameof(LongRunningDebugDocumentSender), logger, TargetType.List.ToArray())
    {
        this.logger = logger;
        this.delayInMs = delayInMs;
        this.failPercentage = failPercentage;
    }

    public override async Task Send(string file, Target target, CancellationToken cancellationToken)
    {
        await Task.Delay(delayInMs, cancellationToken);

        if (failPercentage != 0 && RandomNumberGenerator.GetInt32(0, 100) <= failPercentage)
        {
            throw new DocumentProcessorException($"Test processing document failure from {nameof(LongRunningDebugTemplateProcessor)}");
        }

        await base.Send(file, target, cancellationToken);
    }
}
