using System;

namespace Sopheon.CloudNative.Environments.Domain.Exceptions
{
   public class CloudServiceException : Exception
   {
      public CloudServiceException()
      {
      }

      public CloudServiceException(string message)
          : base(message)
      {
      }

      public CloudServiceException(string message, Exception inner)
          : base(message, inner)
      {
      }
   }
}
