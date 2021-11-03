namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class ProductItemTypeDto
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public bool IsSystem()
      {
         return Id < 0;
      }
   }
}
