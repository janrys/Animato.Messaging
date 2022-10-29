namespace Animato.Messaging.Application.Common.Interfaces;

using System;
using Animato.Messaging.Application.Exceptions;
using Animato.Messaging.Domain.Entities;

public static class ITemplateProcessorExtensions
{
    public static bool ThrowExceptionIfCannotProcess(this ITemplateProcessor processor, DocumentTemplate documentTemplate)
    {
        if (processor is null)
        {
            throw new ArgumentNullException(nameof(processor));
        }

        if (documentTemplate is null)
        {
            throw new ArgumentNullException(nameof(documentTemplate));
        }

        if (!processor.CanProcess(documentTemplate.TargetType))
        {
            throw new ValidationException(
                ValidationException.CreateFailure(nameof(documentTemplate.TargetType)
                , $"Processor with id {processor.Id} cannot process type {documentTemplate.TargetType}"));
        }

        return true;
    }
}
