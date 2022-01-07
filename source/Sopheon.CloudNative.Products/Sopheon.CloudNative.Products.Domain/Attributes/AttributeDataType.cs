

namespace Sopheon.CloudNative.Products.Domain
{
   /// <summary>
   /// NOTE: Changing this enum will require EF Migrations to be generated.
   /// </summary>
   public enum AttributeDataTypes
   {
      String = 1,
      Int32 = 2,
      Decimal = 3,
      Money = 4,
      UtcDateTime = 5,
      MarkdownString = 6,
      EnumCollection = 7, // list of constant values, tied to a numeric Id
      Enum = 8 // constant value, from allowed list of constant values, tied to a numeric Id
   }

   public class AttributeDataType
   {
      /// <summary>
      /// System Defined Values found in enum <see cref="AttributeDataTypes" />
      /// </summary>
      public int AttributeDataTypeId { get; set; }

      public string Name { get; set; }
   }
}
