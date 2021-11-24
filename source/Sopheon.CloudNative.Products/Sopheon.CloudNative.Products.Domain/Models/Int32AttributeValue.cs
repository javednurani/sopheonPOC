namespace Sopheon.CloudNative.Products.Domain
{
   public class Int32AttributeValue : IAttributeValue<int?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public int? Value { get; set; }
   }
}
