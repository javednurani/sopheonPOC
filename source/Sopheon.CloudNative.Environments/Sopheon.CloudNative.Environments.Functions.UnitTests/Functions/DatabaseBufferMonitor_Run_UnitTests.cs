using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class DatabaseBufferMonitor_Run_UnitTests : FunctionUnitTestBase
   {
      private readonly DatabaseBufferMonitor _sut;
      private readonly Mock<IDatabaseBufferMonitorHelper> _mockMonitorHelper;
      private readonly IConfiguration _configuration;

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
      public async Task Run_HappyPath_CallsHelperOnce()
      {
         // Arrange

         // Act
         await _sut.Run(null, string.Empty, _context.Object);

         // Assert
         _mockMonitorHelper.Verify(mh => mh.EnsureDatabaseBufferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
      }

      [Fact]
      public async Task Run_HappyPath_ReplacesTemplateCorrectly()
      {
         // Arrange
         string sqlServerName = Some.Random.String();
         string adminEnigma = _configuration["SqlServerAdminEnigma"];
         Environment.SetEnvironmentVariable("AzSqlServerName", sqlServerName);

         string template = $"{StringConstants.SERVER_NAME_TOKEN}{StringConstants.ADMINISTRATOR_LOGIN_ENIGMA_TOKEN}";

         // Act
         await _sut.Run(null, template, _context.Object);

         // Assert
         string expectedFormattedJson = template
            .Replace(StringConstants.SERVER_NAME_TOKEN, sqlServerName)
            .Replace(StringConstants.ADMINISTRATOR_LOGIN_ENIGMA_TOKEN, adminEnigma);
         _mockMonitorHelper.Verify(mh => mh.EnsureDatabaseBufferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expectedFormattedJson), Times.Once);
      }

      [Fact]
      public async Task Run_ExceptionThrownByDependency_RethrowsException()
      {
         // Arrange
         string exceptionMessage = Some.Random.String();
         Exception ex = new Exception(exceptionMessage);
         _mockMonitorHelper.Setup(mh => mh.EnsureDatabaseBufferAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(ex);

         // Act + Assert
         Exception actualException = await Assert.ThrowsAsync<Exception>(() => _sut.Run(null, string.Empty, _context.Object));
         Assert.Equal(exceptionMessage, actualException.Message);
      }
   }
}
