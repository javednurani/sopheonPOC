using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class DatabaseBufferMonitorHelper_CheckHasDatabaseThreshold_UnitTests
   {
      [Fact]
      public async void DatabaseBufferMonitorHelper_CheckHasDatabaseThreshold_HappyPath()
      {
         // Arrange
         Mock<ILogger<DatabaseBufferMonitorHelper>> logger = new Mock<ILogger<DatabaseBufferMonitorHelper>>();
         Mock<IAzure> azure = new Mock<IAzure>();
         Mock<ISqlServers> mockSqlServers= new Mock<ISqlServers>();
         Mock<ISqlDatabaseOperations> mockDbOperations = new Mock<ISqlDatabaseOperations>();
         Mock<ISqlDatabase> mockDb = new Mock<ISqlDatabase>();
         Mock<IReadOnlyDictionary<string, string>> mockDbTags = new Mock<IReadOnlyDictionary<string, string>>();

         azure.Setup(a => a.SqlServers).Returns(mockSqlServers.Object);
         azure.Setup(a => a.Deployments).Returns(new Mock<IDeployments>().Object);
         var expectedValue = "NotAssigned";
         mockDbTags.Setup(t => t.TryGetValue(It.IsAny<string>(), out expectedValue)).Returns(true);
         mockDb.Setup(db => db.Tags).Returns(mockDbTags.Object);
         mockDbOperations
            .Setup(dbo => dbo.GetBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockDb.Object));
         mockDbOperations
            .Setup(dbo => dbo.ListBySqlServerAsync(It.IsAny<ISqlServer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyList<ISqlDatabase>>(new List<ISqlDatabase> { mockDb.Object }.AsReadOnly())); // could add more dbs here to get inside exit early switch
         mockSqlServers.Setup(s => s.Databases).Returns(mockDbOperations.Object);
         mockSqlServers.Setup(s => s.ElasticPools).Returns(new Mock<ISqlElasticPoolOperations>().Object);

         DatabaseBufferMonitorHelper sut = new DatabaseBufferMonitorHelper(logger.Object, azure.Object);
         
         // Act
         bool result = await sut.CheckHasDatabaseThreshold();

         // Assert
         Assert.True(result);
      }
   }
}
