using System.ComponentModel.DataAnnotations;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class ProductGoalDto
   {
      public int Id { get; set; }

      [Required()]
      [MaxLength(ModelConstraints.NAME_LENGTH_300)]
      public string Name { get; set; }

      public string Description { get; set; }
   }
}
