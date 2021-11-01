using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Helpers
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests
   {
      private readonly ResourceAllocationHelper _sut;
      private readonly Mock<IEnvironmentCommands> _mockEnvironmentCommands;
      private readonly Mock<IAzure> _mockAzure;

      public AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests()
      {
         _mockEnvironmentCommands = new Mock<IEnvironmentCommands>();
         _mockAzure = new Mock<IAzure>();

         _sut = new ResourceAllocationHelper(
            new Mock<ILogger<ResourceAllocationHelper>>().Object,
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

         // Act
         Guid environmentKey = Some.Random.Guid();
         await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, null, null, null);

         // Assert
         _mockEnvironmentCommands.Verify(
            ec => ec.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, databaseName),
            Times.Once);
      }
   }
}
