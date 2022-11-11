namespace Animato.Messaging.Infrastructure.Services.DocumentProcessing;

using Animato.Messaging.Application.Common.Logging;
using Animato.Messaging.Domain.Enums;
using Fluid;
using Microsoft.Extensions.Logging;

public class FluidTemplateProcessor : BaseTemplateProcessor
{
    private readonly ILogger<FluidTemplateProcessor> logger;
    private static readonly FluidParser Parser;

    static FluidTemplateProcessor()
    {
        TemplateOptions.Default.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
        Parser = new FluidParser();
    }

    public FluidTemplateProcessor(ILogger<FluidTemplateProcessor> logger) : base("cefd22f9-6960-4575-9ee5-d68e0b97b552", "", TargetType.List.ToArray())
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override Task<string> Generate(Stream templateStream, object data, TargetType targetType, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var streamReader = new StreamReader(templateStream);
        var templateContent = streamReader.ReadToEnd();

        if (Parser.TryParse(templateContent, out var fluidTemplate, out var error))
        {
            var context = new TemplateContext(data);

            return Task.FromResult(fluidTemplate.Render(context));
        }
        else
        {
            logger.TemplateProcessorError(error);
            throw new DocumentProcessorException($"Error generating document from template, error {error}");
        }
    }
}
