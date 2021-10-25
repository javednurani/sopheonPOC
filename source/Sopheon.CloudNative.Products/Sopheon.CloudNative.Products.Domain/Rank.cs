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
