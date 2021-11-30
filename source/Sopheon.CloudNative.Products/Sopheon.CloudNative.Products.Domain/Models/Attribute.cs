#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Sopheon.CloudNative.Products.Domain
{
   public class Attribute
   {
      public int AttributeId { get; set; }

      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeValueTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public AttributeValueType AttributeValueType { get; set; }

      public string Name { get; set; }

      public string? ShortName { get; set; }

      public bool IsSystem()
      {
         return AttributeId < 0;
      }
   }
}
