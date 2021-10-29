using System;

namespace Sopheon.CloudNative.Environments.Domain.Infrastructure
{
   /// <summary>
   ///  The purpose of this attribute is to represent the ENV.DomainResourceTypes.IsDedicated bool value
   ///  on the C# enum Sopheon.CloudNative.Environments.Domain.Enums.ResourceType
   /// </summary>
   [AttributeUsage(AttributeTargets.Field)]
   public class DedicatedAttribute : Attribute
   {
      private bool _isDedicated;

      public DedicatedAttribute(bool isDedicated = false)
      {
         _isDedicated = isDedicated;
      }

      public bool IsDedicated
      {
         get
         {
            return _isDedicated;
         }
      }
   }
}
