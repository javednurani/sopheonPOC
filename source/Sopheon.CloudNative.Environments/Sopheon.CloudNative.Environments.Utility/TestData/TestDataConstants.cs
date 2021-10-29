using System;

namespace Sopheon.CloudNative.Environments.Utility.TestData
{
   public static class TestDataConstants
   {
      public const string BUSINESS_SERVICE_NAME_1 = "PRODUCT_SERVICE";
      public const string BUSINESS_SERVICE_NAME_2 = "PLANNING_SERVICE";

      public const string DEPENDENCY_NAME_1 = "PRODUCT_SQL_DATASTORE";
      public const string DEPENDENCY_NAME_2 = "PLANNING_SQL_DATASTORE";

      public const string RESOURCE_URI_1 = "https://resource1.database.windows.net";
      public const string RESOURCE_URI_2 = "https://resource2.database.windows.net";
      public const string RESOURCE_URI_3 = "https://resource3.database.windows.net";
      public const string RESOURCE_URI_4 = "https://resource4.database.windows.net";

      private static readonly Guid _environmentKey1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
      private static readonly Guid _environmentKey2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
      private static readonly Guid _environmentKey3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

      public static Guid EnvironmentKey1
      {
         get
         {
            return _environmentKey1;
         }
      }

      public static Guid EnvironmentKey2
      {
         get
         {
            return _environmentKey2;
         }
      }

      public static Guid EnvironmentKey3
      {
         get
         {
            return _environmentKey3;
         }
      }
   }
}
