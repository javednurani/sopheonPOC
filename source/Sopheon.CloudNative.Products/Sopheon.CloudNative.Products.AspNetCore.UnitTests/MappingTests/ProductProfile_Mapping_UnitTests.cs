using AutoMapper;
using Sopheon.CloudNative.Products.AspNetCore.MappingProfiles;
using Xunit;

namespace Sopheon.CloudNative.Products.AspNetCore.UnitTests
{
   public class ProductProfile_Mapping_UnitTests
   {
      [Fact]
      public void AutoMapper_Configuration_IsValid()
      {
         // Arrange + Act
         var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());

         // Assert
         config.AssertConfigurationIsValid();
      }
   }
}
