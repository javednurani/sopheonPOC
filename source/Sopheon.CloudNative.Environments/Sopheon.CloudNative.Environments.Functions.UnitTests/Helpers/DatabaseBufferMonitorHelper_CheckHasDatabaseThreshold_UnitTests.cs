using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Deployment.Definition;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Helpers
{
   public class DatabaseBufferMonitorHelper_CheckHasDatabaseThreshold_UnitTests
   {
      private DatabaseBufferMonitorHelper _sut;
      private Mock<ILogger<DatabaseBufferMonitorHelper>> _logger;
      private Mock<IAzure> _azure;

      public DatabaseBufferMonitorHelper_CheckHasDatabaseThreshold_UnitTests()
      {
         _logger = new Mock<ILogger<DatabaseBufferMonitorHelper>>();
         _azure = new Mock<IAzure>();

         _sut = new DatabaseBufferMonitorHelper(_logger.Object, _azure.Object);
      }

      [Fact]
      public async Task CheckHasDatabaseThreshold_EnoughUnassignedDatabasesExist_DeploymentIsNotCreated()
      {
         // Arrange

         // 5 unassigned databases means no need to run deployment
         SetupMockDatabases(5);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment();

         // Act
         _ = await _sut.CheckHasDatabaseThreshold(null, null, null, null);

         // Assert
         deploymentMock.Verify(wc => wc.Create(), Times.Never, "Should not have created deployment!");
      }

      [Fact]
      public async Task CheckHasDatabaseThreshold_NotEnoughUnassignedDatabasesExist_DeploymentIsCreated()
      {
         // Arrange

         // 4 unassigned databases means we need to deploymore databases
         SetupMockDatabases(4);

         Mock<IWithCreate> deploymentMock = SetupMockDeployment();
         
         // Act
         _ = await _sut.CheckHasDatabaseThreshold(null, null, null, null);

         // Assert
         deploymentMock.Verify(wc => wc.Create(), Times.Once, "Should have created deployment!");
      }

      private void SetupMockDatabases(int numUnassignedDatabases)
      {
         Mock<ISqlServers> mockSqlServers = new Mock<ISqlServers>();
         Mock<ISqlDatabaseOperations> mockDbOperations = new Mock<ISqlDatabaseOperations>();
         Mock<ISqlDatabase> mockDb = new Mock<ISqlDatabase>();
         Mock<IReadOnlyDictionary<string, string>> mockDbTags = new Mock<IReadOnlyDictionary<string, string>>();

         List<ISqlDatabase> unassignedDatabases = new List<ISqlDatabase>(Enumerable.Repeat(mockDb.Object, numUnassignedDatabases));

         _azure.Setup(a => a.SqlServers).Returns(mockSqlServers.Object);
         var expectedValue = "NotAssigned";
         mockDbTags.Setup(t => t.TryGetValue(It.IsAny<string>(), out expectedValue)).Returns(true);
         mockDb.Setup(db => db.Tags).Returns(mockDbTags.Object);
         mockDbOperations
            .Setup(dbo => dbo.GetBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockDb.Object));
         mockDbOperations
            .Setup(dbo => dbo.ListBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyList<ISqlDatabase>>(unassignedDatabases.AsReadOnly()));
         mockSqlServers.Setup(s => s.Databases).Returns(mockDbOperations.Object);
         mockSqlServers.Setup(s => s.ElasticPools).Returns(new Mock<ISqlElasticPoolOperations>().Object);
      }

      private Mock<IWithCreate> SetupMockDeployment()
      {
         Mock<IDeployments> mockDeployments = new Mock<IDeployments>();
         _azure.Setup(a => a.Deployments).Returns(mockDeployments.Object);
         Mock<IBlank> mockBlank = new Mock<IBlank>();
         mockDeployments.Setup(d => d.Define(It.IsAny<string>())).Returns(mockBlank.Object);
         Mock<IWithTemplate> mockWithTemplate = new Mock<IWithTemplate>();
         mockBlank.Setup(b => b.WithExistingResourceGroup(It.IsAny<string>())).Returns(mockWithTemplate.Object);
         Mock<IWithParameters> mockWithParameters = new Mock<IWithParameters>();
         mockWithTemplate.Setup(wt => wt.WithTemplate(It.IsAny<string>())).Returns(mockWithParameters.Object);
         Mock<IWithMode> mockWithMode = new Mock<IWithMode>();
         mockWithParameters.Setup(wp => wp.WithParameters(It.IsAny<object>())).Returns(mockWithMode.Object);
         Mock<IWithCreate> mockWithCreate = new Mock<IWithCreate>();
         mockWithMode.Setup(wm => wm.WithMode(It.IsAny<DeploymentMode>())).Returns(mockWithCreate.Object);

         return mockWithCreate;
      }
   }
}
