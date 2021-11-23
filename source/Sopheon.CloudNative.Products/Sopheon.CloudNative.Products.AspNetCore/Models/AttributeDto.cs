using System.ComponentModel.DataAnnotations;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class AttributeDto
   {
      public int AttributeId { get; set; }

      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeValueTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public AttributeValueTypeDto AttributeValueType { get; set; }

      [Required()]
      [MaxLength(ModelConstraints.NAME_LENGTH_60)]
      public string Name { get; set; }

      public string ShortName { get; set; }

      public bool IsSystem()
      {
         return AttributeId < 0;
      }
   }
}
