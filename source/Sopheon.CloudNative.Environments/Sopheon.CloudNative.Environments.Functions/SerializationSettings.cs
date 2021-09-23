using System.Text.Json;

namespace Sopheon.CloudNative.Environments.Functions
{
   public static class SerializationSettings
   {
      private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

      public static JsonSerializerOptions JsonSerializerOptions
      {
         get
         {
            return _jsonSerializerOptions;
         }
      }
   }
}
