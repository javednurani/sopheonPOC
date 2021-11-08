namespace Sopheon.CloudNative.Products.Domain
{
   public interface IAttributeValue<TValueType>
   {
      int AttributeId { get; set; }

      Attribute Attribute { get; set; }

      TValueType Value { get; set; }
   }
}
