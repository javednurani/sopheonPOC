using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Helpers
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_UnitTests
   {
      private readonly AllocateSqlDatabaseSharedByServicesToEnvironmentHelper _sut;
      private readonly Mock<IEnvironmentCommands> _mockEnvironmentCommands;
      private readonly Mock<IAzure> _mockAzure;
      private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
      private readonly Mock<HttpClient> _mockHttpClient;

      public AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_UnitTests()
      {
         _mockEnvironmentCommands = new Mock<IEnvironmentCommands>();
         _mockAzure = new Mock<IAzure>();
         _mockHttpClientFactory = new Mock<IHttpClientFactory>();
         _mockHttpClient = new Mock<HttpClient>();
         _mockHttpClientFactory
            .Setup(m => m.CreateClient(StringConstants.HTTP_CLIENT_NAME_AZURE_REST_API))
            .Returns(_mockHttpClient.Object);

         _sut = new AllocateSqlDatabaseSharedByServicesToEnvironmentHelper(
            new Mock<ILogger<AllocateSqlDatabaseSharedByServicesToEnvironmentHelper>>().Object,
            _mockHttpClientFactory.Object,
            _mockAzure.Object,
            _mockEnvironmentCommands.Object);
      }

      [Fact]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_HappyPath_CommandCalledWithCorrectArguments()
      {
         // Arrange
         _mockEnvironmentCommands
            .Setup(m => m.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(() =>
            {
               return Task.CompletedTask;
            });
         Mock<ISqlServers> mockSqlServers = new Mock<ISqlServers>();
         _mockAzure.Setup(ma => ma.SqlServers).Returns(mockSqlServers.Object);
         Mock<ISqlDatabaseOperations> mockDbOperations = new Mock<ISqlDatabaseOperations>();
         mockSqlServers.Setup(s => s.Databases).Returns(mockDbOperations.Object);
         Mock<ISqlDatabase> mockDatabase = new Mock<ISqlDatabase>();
         string databaseName = Some.Random.String();
         mockDatabase.Setup(md => md.Name).Returns(databaseName);

         mockDatabase.Setup(db => db.Tags).Returns(
            new Dictionary<string, string>
            {
               { StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL }
            });
         List<ISqlDatabase> availableDatabases = new List<ISqlDatabase>
         {
            mockDatabase.Object
         };

         mockDbOperations
            .Setup(dbo => dbo.ListBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyList<ISqlDatabase>>(availableDatabases.AsReadOnly()));

         mockDbOperations
            .Setup(dbo => dbo.GetBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockDatabase.Object));

         string sqlServerName = Some.Random.String();

         _mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage());

         // Act
         Guid environmentKey = Some.Random.Guid();
         await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, null, null, sqlServerName);

         // Assert
         _mockEnvironmentCommands.Verify(
            ec => ec.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, $"Server=tcp:{sqlServerName}.database.windows.net,1433;Database={databaseName};Encrypt=true;Connection Timeout=30;"),
            Times.Once);
      }
   }
}
