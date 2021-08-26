using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Sopheon.CloudNative.Environments.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Sopheon.CloudNative.Environments.Domain.Models;
using AutoMapper;
using Sopheon.CloudNative.Environments.Functions.Get.Models;

namespace Sopheon.CloudNative.Environments.Functions.Get
{
   public class GetEnvironments
   {
      private readonly EnvironmentContext _environmentContext;
      private IMapper _mapper;

      public GetEnvironments(EnvironmentContext environmentContext, IMapper mapper)
      {
         _environmentContext = environmentContext;
         _mapper = mapper;
      }

      [Function(nameof(GetEnvironments))]
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(GetEnvironments));
         logger.LogInformation("GetEnvironments request.");

         List<Environment> environments = await _environmentContext.Environments.ToListAsync();    

         HttpResponseData okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
         await okResponse.WriteAsJsonAsync(_mapper.Map<List<Environment>, List<EnvironmentDTO>>(environments));
         return okResponse;
      }
   }
}
