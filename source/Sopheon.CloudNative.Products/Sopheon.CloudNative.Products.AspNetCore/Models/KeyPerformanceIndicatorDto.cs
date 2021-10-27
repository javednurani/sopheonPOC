namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   /// <summary>
   /// Can only be created from attributes with numeric AttributeValueTypes
   /// </summary>
   public class KeyPerformanceIndicatorDto
   {
      public int KeyPerformanceIndicatorId { get; set; }

      public int AttributeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public AttributeDto Attribute { get; set; }

      // TODO: Other details that deal with the specific KPI concern, rather than just the attribute 'value holder' 
      // Min/Max?
      // Target?
   }
}
