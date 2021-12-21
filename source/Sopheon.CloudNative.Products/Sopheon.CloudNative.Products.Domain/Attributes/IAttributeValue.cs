namespace Sopheon.CloudNative.Products.Domain
{
   public interface IAttributeValue<TValueType>
   {
      int AttributeId { get; set; }

      ///// <summary>
      ///// Navigation Property
      ///// </summary>
      Attribute Attribute { get; set; }

      TValueType Value { get; set; }
   }
}
