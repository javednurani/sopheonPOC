using System;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.StandAlone;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public abstract class DataDependentFunctionIntegrationTest : FunctionIntegrationTest
   {
      protected static Guid _environmentKey = new Guid("EBA2CCBB-89D3-45E3-BF90-2DB160BF1552");
      protected static string _businessServiceName = "CommentService";
      protected static string _businessServiceDependencyName = "SqlDatabase";
   }
}
