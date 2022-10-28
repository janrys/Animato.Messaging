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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentTemplate>))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplate))]
    [HttpGet("{id}", Name = "GetTemplateById")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }
        DocumentTemplateId templateId;
        if (Guid.TryParse(id, out var parsedTemplateId))
        {
            templateId = new DocumentTemplateId(parsedTemplateId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplate))]
    [HttpPost(Name = "CreateTemplate")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateDocumentTemplateModel template, CancellationToken cancellationToken)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
    [HttpPut("{id}", Name = "UpdateTemplate")]
    public async Task<IActionResult> UpdateTemplate(string id, [FromBody] CreateDocumentTemplateModel template, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }

        DocumentTemplateId documentTemplateId;
        if (Guid.TryParse(id, out var parsedDocumentTemplateId))
        {
            documentTemplateId = new DocumentTemplateId(parsedDocumentTemplateId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
        }

        if (template is null)
        {
            return BadRequest($"{nameof(template)} must have a value");
        }

        var command = new UpdateDocumentTemplateCommand(documentTemplateId, template, GetUser());
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
    [HttpPut("{id}/content", Name = "UpdateTemplateContent")]
    public async Task<IActionResult> UpdateTemplateContent(string id, IFormFile file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }

        if (file is null)
        {
            return BadRequest($"{nameof(file)} must have a value");
        }

        DocumentTemplateId documentTemplateId;
        if (Guid.TryParse(id, out var parsedDocumentTemplateId))
        {
            documentTemplateId = new DocumentTemplateId(parsedDocumentTemplateId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
        }

        var command = new UpdateDocumentTemplateContentCommand(documentTemplateId, file.FileName, file.OpenReadStream(), GetUser());
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
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest($"{nameof(id)} must have a value");
        }

        DocumentTemplateId documentTemplateId;
        if (Guid.TryParse(id, out var parsedDocumentTemplateId))
        {
            documentTemplateId = new DocumentTemplateId(parsedDocumentTemplateId);
        }
        else
        {
            return BadRequest($"{nameof(id)} has a wrong format '{id}'");
        }

        var command = new DeleteDocumentTemplateCommand(documentTemplateId, GetUser());
        await Send(command, cancellationToken);
        return Ok();
    }
}
