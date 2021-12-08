

namespace Sopheon.CloudNative.Products.Domain
{
   public class Attribute
   {
      public int AttributeId { get; set; }

      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeDataTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public AttributeDataType AttributeDataType { get; set; }

      public string Name { get; set; }

      // TODO - confirm nullable DB column, remove entity config if no longer needed
      public string ShortName { get; set; }

      public bool IsSystem()
      {
         return AttributeId < 0;
      }

      public virtual Attribute ShallowCopy()
      {
         return (Attribute)this.MemberwiseClone();
      }
   }
}
