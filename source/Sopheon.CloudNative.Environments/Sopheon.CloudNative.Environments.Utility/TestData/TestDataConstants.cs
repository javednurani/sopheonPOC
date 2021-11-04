using System;

namespace Sopheon.CloudNative.Environments.Utility.TestData
{
   public static class TestDataConstants
   {
      public const string RESOURCE_URI_1 = "Server=https://customerDbServer1.database.windows.net; Database=dwjksg5cydttc-11;";
      public const string RESOURCE_URI_2 = "Server=https://customerDbServer1.database.windows.net; Database=dwjksg5cydttc-74;";
      public const string RESOURCE_URI_3 = "Server=https://customerDbServer1.database.windows.net; Database=dwjksg5cydttc-26;";
      public const string RESOURCE_URI_4 = "Server=https://customerDbServer2.database.windows.net; Database=sfkdmh7bies5s-96;";
      public const string RESOURCE_URI_5 = "DefaultEndpointsProtocol=https;AccountName=sharedCustomerStorage1;EndpointSuffix=core.windows.net";
      public const string RESOURCE_URI_6 = "DefaultEndpointsProtocol=https;AccountName=sharedCustomerStorage2;EndpointSuffix=core.windows.net";

      private static readonly Guid _environmentKey1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
      private static readonly Guid _environmentKey2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
      private static readonly Guid _environmentKey3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

      private static readonly Guid _ownerKey1 = Guid.Parse("00000000-0000-0000-0000-000000000008");

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

      public static Guid OwnerKey1
      {
         get
         {
            return _ownerKey1;
         }
      }
   }
}
