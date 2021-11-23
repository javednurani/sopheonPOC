#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Sopheon.CloudNative.Products.Domain
{
   public class Goal
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string? Description { get; set; }
   }
}
