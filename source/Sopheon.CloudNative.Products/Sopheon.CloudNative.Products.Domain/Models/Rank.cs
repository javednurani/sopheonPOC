#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Sopheon.CloudNative.Products.Domain
{
   public class Rank
   {
      public int RankId { get; set; }

      public string Value { get; set; }
   }

   public interface IRankedEntity
   {
      Rank Rank { get; set; }
   }
}
