
namespace Sopheon.CloudNative.Environments.Domain
{
   public static class ModelConstraints
   {
      public static int NAME_LENGTH = 64;
      public static int DESCRIPTION_LENGTH = 1024;  
      public static int URI_LENGTH = 450; // 900 byte limit for indices, unicode chars take up 2 bytes each
   }
}
