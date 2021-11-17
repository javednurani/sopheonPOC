using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Deployment.Definition;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Helpers
{
   public class DatabaseBufferMonitorHelper_EnsureDatabaseBufferAsync_UnitTests
   {
      private DatabaseBufferMonitorHelper _sut;
      private Mock<ILogger<DatabaseBufferMonitorHelper>> _logger;
      private Mock<IAzure> _azure;

      public DatabaseBufferMonitorHelper_EnsureDatabaseBufferAsync_UnitTests()
      {
         _logger = new Mock<ILogger<DatabaseBufferMonitorHelper>>();
         _azure = new Mock<IAzure>();
         System.Environment.SetEnvironmentVariable("DatabaseBufferCapacity", "5");
         _sut = new DatabaseBufferMonitorHelper(_logger.Object, _azure.Object);
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_HappyPath_DeploymentIsCreated()
      {
         // Arrange

         // 4 unassigned databases means we need to deploymore databases
         SetupMockDatabases(4);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment(); // activeDeploymentsExistForResourceGroup = false

         // Act
         await _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String());

         // Assert
         deploymentMock.Verify(wc => wc.BeginCreateAsync(default(CancellationToken)), Times.Once, "Should have created deployment!");
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_ActiveDeploymentExists_DeploymentIsNotCreated()
      {
         // Arrange

         // 4 unassigned databases means we need to deploy more databases
         SetupMockDatabases(4);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment(existingDeploymentIsActive: true);

         // Act
         await _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String());

         // Assert
         deploymentMock.Verify(wc => wc.BeginCreateAsync(default(CancellationToken)), Times.Never, "Should not have created deployment!");
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_EnoughUnassignedDatabasesExist_DeploymentIsNotCreated()
      {
         // Arrange

         // 5 unassigned databases means no need to run deployment
         SetupMockDatabases(5);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment();

         // Act
         await _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String());

         // Assert
         deploymentMock.Verify(wc => wc.BeginCreateAsync(default(CancellationToken)), Times.Never, "Should not have created deployment!");
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_DatabaseDeletedDuringCheck_DeletedDatabaseTagsNotChecked()
      {
         // Arrange

         // 10 unassigned but deleted dbs means we need to deploy more becuase they no longer exist
         SetupMockDatabases(10, true);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment();

         // Act
         await _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String());

         // Assert
         deploymentMock.Verify(wc => wc.BeginCreateAsync(default(CancellationToken)), Times.Once, "Should have created deployment!");
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_DatabaseBufferCapacityInvalid_ExceptionThrownNoDeployment()
      {
         // Arrange
         System.Environment.SetEnvironmentVariable("DatabaseBufferCapacity", "badValue");
         // 10 unassigned but deleted dbs means we need to deploy more becuase they no longer exist
         SetupMockDatabases(4);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment();

         // Act

         // Assert
         await Assert.ThrowsAsync<ArgumentException>(() => _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String()));
         deploymentMock.Verify(wc => wc.BeginCreateAsync(default(CancellationToken)), Times.Never, "Should not have created deployment!");
      }

      [Fact]
      public async Task EnsureDatabaseBufferAsync_AzureSqlServerNotFound_CloudServiceExceptionThrown()
      {
         // Arrange
         SetupMockDatabases(4, serverNotFound: true);
         Mock<IWithCreate> deploymentMock = SetupMockDeployment();

         // Act

         // Assert
         await Assert.ThrowsAsync<CloudServiceException>(() => _sut.EnsureDatabaseBufferAsync(Some.Random.String(), Some.Random.String(), Some.Random.String(), Some.Random.String()));
      }

      private void SetupMockDatabases(int numUnassignedDatabases, bool databasesDeleted = false, bool serverNotFound = false)
      {
         Mock<ISqlServer> mockSqlServer = new Mock<ISqlServer>();
         Mock<ISqlServers> mockSqlServers = new Mock<ISqlServers>();
         Mock<ISqlDatabaseOperations> mockDbOperations = new Mock<ISqlDatabaseOperations>();
         Mock<ISqlDatabase> mockDb = new Mock<ISqlDatabase>();
         Mock<IReadOnlyDictionary<string, string>> mockDbTags = new Mock<IReadOnlyDictionary<string, string>>();

         List<ISqlDatabase> unassignedDatabases = new List<ISqlDatabase>(Enumerable.Repeat(mockDb.Object, numUnassignedDatabases));

         _azure.Setup(a => a.SqlServers).Returns(mockSqlServers.Object);
         var expectedValue = "NotAssigned";
         mockDbTags.Setup(t => t.TryGetValue(It.IsAny<string>(), out expectedValue)).Returns(true);
         mockDb.Setup(db => db.Tags).Returns(mockDbTags.Object);
         mockDb.Setup(db => db.Name).Returns(Some.Random.String());
         if (!databasesDeleted)
         { 
            mockDbOperations
               .Setup(dbo => dbo.GetBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(mockDb.Object));
         }
         mockDbOperations
            .Setup(dbo => dbo.ListBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyList<ISqlDatabase>>(unassignedDatabases.AsReadOnly()));
         mockSqlServers.Setup(s => s.Databases).Returns(mockDbOperations.Object);
         mockSqlServers.Setup(s => s.ElasticPools).Returns(new Mock<ISqlElasticPoolOperations>().Object);
         if (serverNotFound)
         {
            mockSqlServers.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<ISqlServer>(null));
         }
         else
         {
            mockSqlServers.Setup(s => s.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockSqlServer.Object);
         }
      }

      private Mock<IWithCreate> SetupMockDeployment(bool existingDeploymentIsActive = false)
      {
         Mock<IDeployments> mockDeployments = new Mock<IDeployments>();
         _azure.Setup(a => a.Deployments).Returns(mockDeployments.Object);

         Mock<IDeployment> existingDeployment = new Mock<IDeployment>();
         existingDeployment.Setup(d => d.Name).Returns($"{nameof(DatabaseBufferMonitor)}_{Some.Random.String()}");
         existingDeployment
            .Setup(d => d.ProvisioningState)
            .Returns(
               existingDeploymentIsActive
                  ? ProvisioningState.Accepted
                  : ProvisioningState.Succeeded);

         IPagedCollection<IDeployment> deploymentsFromResourceGroup =
            PagedCollection<IDeployment, IDeployment>.CreateFromEnumerable(new List<IDeployment> { existingDeployment.Object });

         mockDeployments
            .Setup(d => d.ListByResourceGroupAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(deploymentsFromResourceGroup));

         Mock<IBlank> mockBlank = new Mock<IBlank>();
         mockDeployments.Setup(d => d.Define(It.IsAny<string>())).Returns(mockBlank.Object);

         Mock<IWithTemplate> mockWithTemplate = new Mock<IWithTemplate>();
         mockBlank.Setup(b => b.WithExistingResourceGroup(It.IsAny<string>())).Returns(mockWithTemplate.Object);

         Mock<IWithParameters> mockWithParameters = new Mock<IWithParameters>();
         mockWithTemplate.Setup(wt => wt.WithTemplate(It.IsAny<string>())).Returns(mockWithParameters.Object);

         Mock<IWithMode> mockWithMode = new Mock<IWithMode>();
         mockWithParameters.Setup(wp => wp.WithParameters(It.IsAny<string>())).Returns(mockWithMode.Object);

         Mock<IWithCreate> mockWithCreate = new Mock<IWithCreate>();
         mockWithMode.Setup(wm => wm.WithMode(It.IsAny<DeploymentMode>())).Returns(mockWithCreate.Object);

         return mockWithCreate;
      }
   }
}
