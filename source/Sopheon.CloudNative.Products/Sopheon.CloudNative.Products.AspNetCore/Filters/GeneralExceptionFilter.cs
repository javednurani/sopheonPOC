using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Sopheon.CloudNative.Products.AspNetCore.Filters
{
   public class GeneralExceptionFilter : IExceptionFilter
   {
      private readonly IHostEnvironment _hostEnvironment;

      public GeneralExceptionFilter(IHostEnvironment hostEnvironment)
      {
         _hostEnvironment = hostEnvironment;
      }

      public void OnException(ExceptionContext context)
      {
         context.Result = new ContentResult
         {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Content = _hostEnvironment.IsDevelopment()
               ? context.Exception.ToString()
               : "Something went wrong.  Please try again later." // TODO, remove hardcoded string
         };
      }
   }
}
