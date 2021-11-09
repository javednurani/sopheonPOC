using System;
using Sopheon.CloudNative.Environments.Domain.Enums;

namespace Sopheon.CloudNative.Environments.Domain.Infrastructure
{
   [AttributeUsage(AttributeTargets.Field)]
   public class BusinessServiceAttribute : Attribute
   {
      private BusinessServices _businessService;

      public BusinessServiceAttribute(BusinessServices businessService)
      {
         _businessService = businessService;
      }

      public BusinessServices BusinessService
      {
         get
         {
            return _businessService;
         }
      }
   }
}