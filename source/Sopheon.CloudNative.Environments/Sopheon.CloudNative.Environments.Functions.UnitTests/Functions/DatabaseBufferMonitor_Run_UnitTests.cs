using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class DatabaseBufferMonitor_Run_UnitTests : FunctionUnitTestBase
   {
      public DatabaseBufferMonitor_Run_UnitTests()
      {
      }

      [Fact]
      public void DatabaseBufferMonitor_Run_HappyPath()
      {
         // Arrange + Act
         new DatabaseBufferMonitor(null).Run(null, _context.Object);

         //Assert
         Assert.True(true);
      }
   }
}
