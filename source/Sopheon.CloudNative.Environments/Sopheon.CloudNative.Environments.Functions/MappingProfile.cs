using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Functions.Models;
using Profile = AutoMapper.Profile;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<DomainResourceType, ResourceTypeDto>()
            .ReverseMap();

         CreateMap<Resource, ResourceDto>()
            .ForMember((resourceDto => resourceDto.ResourceTypeId),
               memberOptions => memberOptions.MapFrom(resource => resource.DomainResourceTypeId))
            .ReverseMap();

         CreateMap<BusinessServiceDependency, BusinessServiceDependencyDto>().ReverseMap();

         CreateMap<Environment, EnvironmentDto>().ReverseMap();
      }
   }
}
