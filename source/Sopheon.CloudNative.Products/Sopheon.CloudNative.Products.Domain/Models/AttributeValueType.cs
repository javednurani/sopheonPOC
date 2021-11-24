namespace Sopheon.CloudNative.Products.Domain
{
   /// <summary>
   /// NOTE: Changing this enum will require EF Migrations to be generated.
   /// </summary>
   public enum AttributeValueTypes
   {
      String = 1,
      Int32 = 2,
      Decimal = 3,
      Money = 4,
      UtcDateTime = 5,
      MarkdownString = 6
   }

   public class AttributeValueType
   {
      /// <summary>
      /// System Defined Values found in enum <see cref="AttributeValueTypes" />
      /// </summary>
      public int AttributeValueTypeId { get; set; }

      public string Name { get; set; }
   }
}
