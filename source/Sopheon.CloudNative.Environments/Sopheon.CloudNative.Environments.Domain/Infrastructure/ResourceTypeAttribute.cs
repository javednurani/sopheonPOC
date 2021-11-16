using System;
using Sopheon.CloudNative.Environments.Domain.Enums;

namespace Sopheon.CloudNative.Environments.Domain.Infrastructure
{
   /// <summary>
   ///  The purpose of this attribute is to represent the ENV.ResourceType associated with a ENV.BusinessServiceDependency
   ///  on the C# enum Sopheon.CloudNative.Environments.Domain.Enums.BusinessServiceDependencies
   /// </summary>
   [AttributeUsage(AttributeTargets.Field)]
   public class ResourceTypeAttribute : Attribute
   {
      private ResourceTypes _resourceType;

      public ResourceTypeAttribute(ResourceTypes resourceType)
      {
         _resourceType = resourceType;
      }

      public ResourceTypes ResourceType
      {
         get
         {
            return _resourceType;
         }
      }
   }
}
