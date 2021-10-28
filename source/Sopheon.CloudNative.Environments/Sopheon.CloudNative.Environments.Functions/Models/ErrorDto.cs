using System.Net;

namespace Sopheon.CloudNative.Environments.Functions.Models
{
   public class ErrorDto
   {
      
      /// <summary>
      /// DTO representing an error or exception response to a request for a JSON return type
      /// </summary>
      /// <param name="statusCode">The HTTP status code detailing the type of error</param>
      /// <param name="message">The message describing the error.</param>
      public ErrorDto(HttpStatusCode statusCode, string message)
      {
         StatusCode = statusCode;
         Message = message;
      }

      public HttpStatusCode StatusCode
      {
         get;
         set;
      }

      public string Message
      {
         get;
         set;
      }
   }
}
