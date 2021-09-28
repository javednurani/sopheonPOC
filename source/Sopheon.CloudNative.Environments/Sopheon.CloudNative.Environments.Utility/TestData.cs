using System;

namespace Sopheon.CloudNative.Environments.Utility
{
   public static class TestData
   {
      public const string BUSINESS_SERVICE_NAME_1 = "PRODUCT_SERVICE";
      public const string DEPENDENCY_NAME_1 = "PRODUCT_DATASTORE";
      public const string RESOURCE_URI_1 = "https://hammer-prod-sql.database.windows.net";

      private static readonly Guid _environmentKey1 = Guid.Parse("6EC0D5D9-EB3B-4AC4-B809-530BF9661238");
      public static Guid EnvironmentKey1
      {
         get
         {
            return _environmentKey1;
         }
      }
   }
}
