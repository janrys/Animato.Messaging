namespace Animato.Messaging.Application.Features.Templates;

using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Entities;
using AutoMapper;

public class TemplateProfile : Profile
{
    public TemplateProfile() => CreateMap<CreateDocumentTemplateModel, DocumentTemplate>();
}
