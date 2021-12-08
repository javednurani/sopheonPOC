

namespace Sopheon.CloudNative.Products.Domain
{
   public class Goal
   {
      public int Id { get; set; }

      public string Name { get; set; }

      // TODO - confirm nullable DB column, remove entity config if no longer needed
      public string Description { get; set; }
   }
}
