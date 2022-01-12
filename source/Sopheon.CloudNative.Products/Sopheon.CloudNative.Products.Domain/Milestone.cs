namespace Sopheon.CloudNative.Products.Domain
{
   public class Milestone
   {
      public int Id { get; set; }

      public int ProductId { get; set; }

      public string Name { get; set; }

      public string Notes { get; set; }

      public DateTime? Date { get; set; }

      public virtual Product Product { get; set; }
   }
}
