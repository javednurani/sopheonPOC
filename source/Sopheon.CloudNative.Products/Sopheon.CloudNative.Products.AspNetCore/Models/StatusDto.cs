namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class StatusDto
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public bool IsSystem()
      {
         return Id < 0;
      }
   }
}
