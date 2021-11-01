using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Domain.Exceptions
{
   public class CommandFailedException : Exception
   {
      public CommandFailedException()
      {
      }

      public CommandFailedException(string message)
         : base(message)
      {
      }

      public CommandFailedException(string message, Exception inner)
         : base(message, inner)
      {
      }
   }
}
