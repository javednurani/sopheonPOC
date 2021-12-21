using TaskStatus = Sopheon.CloudNative.Products.Domain.Enums.TaskStatus;

namespace Sopheon.CloudNative.Products.Domain
{
   public class Task
   {
      public int Id { get; set; }

      public int ProductId { get; set; }

      public string Name { get; set; }

      public string Notes { get; set; }

      public TaskStatus Status { get; set; }

      public DateTime DueDate { get; set; }

      public Product Product { get; set; }
   }
}
