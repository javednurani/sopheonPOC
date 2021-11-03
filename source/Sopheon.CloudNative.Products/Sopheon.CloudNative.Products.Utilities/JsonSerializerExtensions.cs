using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.Utilities
{
   public static class JsonSerializerExtensions
   {
      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="json"></param>
      /// <param name="anonymousTypeObject"></param>
      /// <param name="options"></param>
      /// <returns></returns>
      public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions options = default)
          => JsonSerializer.Deserialize<T>(json, options);

      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="TValue"></typeparam>
      /// <param name="stream"></param>
      /// <param name="anonymousTypeObject"></param>
      /// <param name="options"></param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      public static ValueTask<TValue> DeserializeAnonymousTypeAsync<TValue>(Stream stream, TValue anonymousTypeObject, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
          => JsonSerializer.DeserializeAsync<TValue>(stream, options, cancellationToken);
   }
}
