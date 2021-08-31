using Sopheon.CloudNative.Environments.Functions.Get.Models;
using Profile = AutoMapper.Profile;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Functions.Get
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Environment, EnvironmentDTO>().ReverseMap();
      }
   }
}
