namespace Animato.Messaging.Application.Features.Templates;

using Animato.Messaging.Application.Features.Templates.Contracts;
using Animato.Messaging.Domain.Entities;
using Animato.Messaging.Domain.Enums;
using AutoMapper;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<CreateDocumentTemplateModel, DocumentTemplate>()
            .ForMember(t => t.TargetType, opt => opt.MapFrom(src => TargetType.FromName(src.TargetType, true)))
            .ForMember(t => t.ProcessorId, opt => opt.MapFrom(src => new ProcessorId(Guid.Parse(src.ProcessorId))));
        CreateMap<DocumentTemplate, DocumentTemplateDto>();
    }
}
