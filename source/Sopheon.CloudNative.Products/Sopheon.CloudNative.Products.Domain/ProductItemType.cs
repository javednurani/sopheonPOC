

namespace Sopheon.CloudNative.Products.Domain
{
   public enum SystemManagedProductItemTypeIds
   {
      Task = -1,
      Feature = -2,
      Risk = -3
   }

   public class ProductItemType
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public bool IsSystem()
      {
         return Id < 0;
      }
   }
}
