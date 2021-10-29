using System;

namespace Sopheon.CloudNative.Environments.Domain.Infrastructure
{
   /// <summary>
   ///  The purpose of this class is to represent the ENV.DomainResourceTypes.IsDedicated bool value
   ///  on the C# enum Sopheon.CloudNative.Environments.Domain.Enums.ResourceType
   /// </summary>
   [AttributeUsage(AttributeTargets.Field)]
   public class DedicatedAttribute : Attribute
   {
      public static readonly DedicatedAttribute Default = new DedicatedAttribute();
      private bool isDedicated;

      public DedicatedAttribute(bool isDedicated = false)
      {
         this.isDedicated = isDedicated;
      }

      public virtual bool IsDedicated
      {
         get
         {
            return IsDedicatedValue;
         }
      }

      protected bool IsDedicatedValue
      {
         get
         {
            return isDedicated;
         }
      }
   }
}
