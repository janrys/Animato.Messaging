namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Domain.Enums;
using Microsoft.Extensions.Logging;

public class DebugTemplateProcessor : BaseTemplateProcessor
{
    private readonly ILogger logger;
    public const string ProcessorId = "a94429b4-1111-1111-aa8a-1ef044162b63";

    protected DebugTemplateProcessor(string id, string name, ILogger logger, params TargetType[] targetTypes) : base(id, name, targetTypes)
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public DebugTemplateProcessor(ILogger<DebugTemplateProcessor> logger) : base(ProcessorId, "", TargetType.List.ToArray())
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override async Task<string> Generate(Stream templateStream, object data, TargetType targetType, CancellationToken cancellationToken)
    {
        using var streamReader = new StreamReader(templateStream);
        var templateContent = await streamReader.ReadToEndAsync();
        templateContent ??= "";
        logger.LogInformation("Processing template by processor {Processor} {ProcessorId} for target {TargetType} data {Data} template {Template}"
            , nameof(DebugTemplateProcessor), Id, targetType.Name, data.ToLogString(), templateContent);
        return $"{data.ToLogString()} {templateContent}";
    }
}

