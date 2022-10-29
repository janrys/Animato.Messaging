namespace Animato.Messaging.Application.Features.TemplateProcessors;

using Animato.Messaging.Application.Common.Interfaces;
using Animato.Messaging.Application.Features.TemplateProcessors.Contracts;
using AutoMapper;

public class TemplateProcessorProfile : Profile
{
    public TemplateProcessorProfile()
        => CreateMap<ITemplateProcessor, RegisteredTemplateProcessor>()
            .ForMember(t => t.TargetTypes, opt => opt.MapFrom(src => src.TargetTypes.Select(t => t.Name)));
}
