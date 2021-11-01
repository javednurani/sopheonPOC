namespace Sopheon.CloudNative.Environments.Functions.Models
{
   public class ErrorDto
   {
      /// <summary>
      /// DTO representing an error or exception response to a request for a JSON return type
      /// </summary>
      /// <param name="httpStatusCode">The HTTP status code detailing the type of error</param>
      /// <param name="message">The message describing the error.</param>
      public ErrorDto(int httpStatusCode, string message)
      {
         HttpStatusCode = httpStatusCode;
         Message = message;
      }

      public int HttpStatusCode
      {
         get;
         init;
      }

      public string Message
      {
         get;
         init;
      }
   }
}
