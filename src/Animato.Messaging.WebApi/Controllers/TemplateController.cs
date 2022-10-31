namespace Animato.Messaging.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Application.Features.Templates;
using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Application.Features.Queues;

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class TemplateController : ApiControllerBase
{
    public TemplateController(ISender mediator) : base(mediator) { }

    /// <summary>
    /// Get all templates
    /// </summary>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>List of templates</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentTemplateDto>))]
    [HttpGet(Name = "GetTemplates")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTemplatesQuery(GetUser());
        var templates = await Send(query, cancellationToken);
        return Ok(templates);
    }

    /// <summary>
    /// Get document template
    /// </summary>
    /// <param name="id">Queue id</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Queue</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDto))]
    [HttpGet("{id}", Name = "GetTemplateById")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateTemplateId(id, out var templateId, out var actionResult))
        {
            return actionResult;
        }

        var query = new GetTemplateByIdQuery(templateId, GetUser());
        var template = await Send(query, cancellationToken);

        if (template is null)
        {
            return NotFound();
        }

        return Ok(template);
    }

    /// <summary>
    /// Create document template
    /// </summary>
    /// <param name="template"></param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Created template</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDto))]
    [HttpPost(Name = "CreateTemplate")]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateDocumentTemplateModel template, CancellationToken cancellationToken)
    {
        if (template is null)
        {
            return BadRequest($"{nameof(template)} must have a value");
        }

        var command = new CreateDocumentTemplateCommand(template, GetUser());
        var createdTemplate = await Send(command, cancellationToken);
        return Ok(createdTemplate);
    }

    /// <summary>
    /// Update document template
    /// </summary>
    /// <param name="id">Template id to update</param>
    /// <param name="template">Template changes</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Updated template</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDto))]
    [HttpPut("{id}", Name = "UpdateTemplate")]
    public async Task<IActionResult> UpdateTemplate(string id, [FromBody] CreateDocumentTemplateModel template, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateTemplateId(id, out var templateId, out var actionResult))
        {
            return actionResult;
        }

        if (template is null)
        {
            return BadRequest($"{nameof(template)} must have a value");
        }

        var command = new UpdateDocumentTemplateCommand(templateId, template, GetUser());
        var updatedTemplate = await Send(command, cancellationToken);
        return Ok(updatedTemplate);
    }

    /// <summary>
    /// Update document template
    /// </summary>
    /// <param name="id">Template id to update</param>
    /// <param name="file">Template content</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Updated template</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDto))]
    [HttpPut("{id}/content", Name = "UpdateTemplateContent")]
    public async Task<IActionResult> UpdateTemplateContent(string id, IFormFile file, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateTemplateId(id, out var templateId, out var actionResult))
        {
            return actionResult;
        }

        if (file is null)
        {
            return BadRequest($"{nameof(file)} must have a value");
        }

        var command = new UpdateDocumentTemplateContentCommand(templateId, file.FileName, file.OpenReadStream(), GetUser());
        var updatedTemplate = await Send(command, cancellationToken);
        return Ok(updatedTemplate);
    }

    /// <summary>
    /// Delete document template
    /// </summary>
    /// <param name="id">Template id to delete</param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id}", Name = "DeleteTemplate")]
    public async Task<IActionResult> DeleteTemplate(string id, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateTemplateId(id, out var templateId, out var actionResult))
        {
            return actionResult;
        }

        var command = new DeleteDocumentTemplateCommand(templateId, GetUser());
        await Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Update document template processor
    /// </summary>
    /// <param name="id">Template id to update</param>
    /// <param name="processorId"></param>
    /// <param name="cancellationToken">Cancelation token</param>
    /// <returns>Updated template</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDto))]
    [HttpPut("{id}/processor/{processorId}", Name = "UpdateTemplateProcessor")]
    public async Task<IActionResult> UpdateTemplateProcessor(string id, string processorId, CancellationToken cancellationToken)
    {
        if (!TryParseAndValidateTemplateId(id, out var templateId, out var actionResult))
        {
            return actionResult;
        }

        ProcessorId validProcessorId;
        if (Guid.TryParse(processorId, out var parsedProcessorId))
        {
            validProcessorId = new ProcessorId(parsedProcessorId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
        }

        var command = new UpdateDocumentTemplateProcessorCommand(templateId, validProcessorId, GetUser());
        var updatedTemplate = await Send(command, cancellationToken);
        return Ok(updatedTemplate);
    }
}
