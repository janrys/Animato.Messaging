namespace Animato.Messaging.Infrastructure.Mappings;

using System.Reflection;

public class MappingProfile : Application.Common.Mappings.MappingProfile
{
    public MappingProfile() => ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

}
