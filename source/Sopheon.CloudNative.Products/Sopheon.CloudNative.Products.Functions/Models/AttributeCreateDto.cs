namespace Sopheon.CloudNative.Products.Api.Models
{
   public class AttributeCreateDto
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeValueTypeId { get; set; }

      public string Name { get; set; }
   }
}
