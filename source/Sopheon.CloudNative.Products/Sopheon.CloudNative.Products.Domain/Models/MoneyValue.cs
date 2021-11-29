#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Sopheon.CloudNative.Products.Domain
{
   public class MoneyValue
   {
      public string CurrencyCode { get; set; }
      public decimal? Value { get; set; }
   }
}
