using Sopheon.CloudNative.Environments.Functions.Models;
using Profile = AutoMapper.Profile;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Environment, EnvironmentDto>().ReverseMap();
      }
   }
}
