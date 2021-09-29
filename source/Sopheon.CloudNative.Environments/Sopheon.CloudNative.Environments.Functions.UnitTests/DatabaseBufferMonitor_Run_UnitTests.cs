﻿using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class DatabaseBufferMonitor_Run_UnitTests : FunctionUnitTestBase
   {
      public DatabaseBufferMonitor_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public void DatabaseBufferMonitor_Run_HappyPath()
      {
         // Arrange + Act
         DatabaseBufferMonitor.Run(null, _context.Object);

         //Assert
         Assert.True(true);
      }

      private void TestSetup()
      {
         SetupFunctionContext();
      }
   }
}
