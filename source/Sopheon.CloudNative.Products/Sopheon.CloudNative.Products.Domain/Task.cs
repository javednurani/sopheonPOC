namespace Sopheon.CloudNative.Products.Domain
{
   public class Task
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string Notes { get; set; }

      // TODO Status enum?
      public int Status { get; set; }

      public DateTime DueDate { get; set; }

      public int ProductId { get; set; }

      public Product Product { get; set; }
   }
}
