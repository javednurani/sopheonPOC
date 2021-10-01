using Microsoft.Extensions.Options;
using System;

namespace Sopheon.CloudNative.Environments.Utility
{
   public class SecretRevealer : ISecretRevealer
   {
      private readonly UserSecretManager _secrets;
      public SecretRevealer(IOptions<UserSecretManager> secrets)
      {
         _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
      }

      public string RevealLocalConnectionString()
      {
         return _secrets.LocalDatabaseConnectionString;
      }
   }
}
