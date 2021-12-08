

namespace Sopheon.CloudNative.Products.Domain
{
   public enum SystemManagedStatusIds
   {
      Open = -1,
      Closed = -2
   }

   public class Status
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public bool IsSystem()
      {
         return Id < 0;
      }
   }
}
