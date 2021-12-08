#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
