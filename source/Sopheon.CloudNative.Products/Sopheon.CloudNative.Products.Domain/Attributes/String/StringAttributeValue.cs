

namespace Sopheon.CloudNative.Products.Domain.Attributes.String
{
   public class StringAttributeValue : IAttributeValue<string>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public string Value { get; set; }
   }
}
