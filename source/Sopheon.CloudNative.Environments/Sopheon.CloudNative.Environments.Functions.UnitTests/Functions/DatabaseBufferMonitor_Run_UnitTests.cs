using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class DatabaseBufferMonitor_Run_UnitTests : FunctionUnitTestBase
   {
      private DatabaseBufferMonitor _sut;
      private Mock<IDatabaseBufferMonitorHelper> _mockMonitorHelper;
      private IConfiguration _configuration;

      public DatabaseBufferMonitor_Run_UnitTests()
      {
         _mockMonitorHelper = new Mock<IDatabaseBufferMonitorHelper>();
         
         Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
             {"SqlServerAdminEnigma", Some.Random.String()}, 
         };
         _configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
             .Build();

         _sut = new DatabaseBufferMonitor(_mockMonitorHelper.Object, _configuration);
      }

      [Fact]
      public void DatabaseBufferMonitor_Run_HappyPath()
      {
         // Arrange

         // Act
         _sut.Run(null, string.Empty, _context.Object);

         // Assert
         _mockMonitorHelper.Verify(mh => mh.EnsureDatabaseBufferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
      }

      [Fact]
      public void DatabaseBufferMonitor_Run_ReplacesTemplateCorrectly()
      {
         // Arrange
         string sqlServerName = Some.Random.String();
         string adminEnigma = _configuration["SqlServerAdminEnigma"];
         Environment.SetEnvironmentVariable("AzSqlServerName", sqlServerName);

         string template = $"{DatabaseBufferMonitor.SERVER_NAME_TOKEN}{DatabaseBufferMonitor.ADMINISTRATOR_LOGIN_ENIGMA_TOKEN}";

         // Act
         _sut.Run(null, template, _context.Object);

         // Assert
         string expectedFormattedJson = template
            .Replace(DatabaseBufferMonitor.SERVER_NAME_TOKEN, sqlServerName)
            .Replace(DatabaseBufferMonitor.ADMINISTRATOR_LOGIN_ENIGMA_TOKEN, adminEnigma);
         _mockMonitorHelper.Verify(mh => mh.EnsureDatabaseBufferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expectedFormattedJson), Times.Once);
      }
   }
}
