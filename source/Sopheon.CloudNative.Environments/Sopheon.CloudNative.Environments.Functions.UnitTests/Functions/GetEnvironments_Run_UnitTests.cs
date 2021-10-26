using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class GetEnvironments_Run_UnitTests : FunctionUnitTestBase
   {
      GetEnvironments Sut;

      public GetEnvironments_Run_UnitTests()
      {
         Sut = new GetEnvironments(_mockEnvironmentRepository.Object, _mapper, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_EnvironmentsReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() =>
         {
            IEnumerable<Environment> environments = new List<Environment>
            {
               new Environment
               {
                  Name = Some.Random.String(),
                  Owner = Some.Random.Guid(),
                  EnvironmentKey = Some.Random.Guid(),
                  Description = Some.Random.String(),
                  IsDeleted = false,
               },
               new Environment
               {
                  Name = Some.Random.String(),
                  Owner = Some.Random.Guid(),
                  EnvironmentKey = Some.Random.Guid(),
                  Description = Some.Random.String(),
                  IsDeleted = false,
               },
            };
            return Task.FromResult(environments);
         });
         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.GetEnvironments(), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         List<EnvironmentDto> environmentResponse = JsonSerializer.Deserialize<List<EnvironmentDto>>(responseBody);

         Assert.NotEmpty(environmentResponse);
      }

      [Fact]
      public async Task Run_HappyPath_NoneReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() =>
         {
            IEnumerable<Environment> environments = new List<Environment>();
            return Task.FromResult(environments);
         });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.GetEnvironments(), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         List<EnvironmentDto> environmentResponse = JsonSerializer.Deserialize<List<EnvironmentDto>>(responseBody);
         Assert.Empty(environmentResponse);
      }
   }
}
