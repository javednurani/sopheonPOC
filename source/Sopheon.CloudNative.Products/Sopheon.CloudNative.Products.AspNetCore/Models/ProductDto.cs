using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class ProductDto
   {
      public int Id { get; set; }

      public List<Int32AttributeValueDto> Int32AttributeValues { get; set; }

      public List<StringAttributeValueDto> StringAttributeValues { get; set; }

      public List<DecimalAttributeValueDto> DecimalAttributeValues { get; set; }

      public List<UtcDateTimeAttributeValueDto> UtcDateTimeAttributeValues { get; set; }

      public List<MoneyAttributeValueDto> MoneyAttributeValues { get; set; }

      public List<KeyPerformanceIndicatorDto> KeyPerformanceIndicators { get; set; }
      public List<EnumAttributeValueDto> EnumAttributeValues { get; set; }
      public List<EnumCollectionAttributeValueDto> EnumCollectionAttributeValues { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      //public int? StatusId { get; set; }

      //public Status Status { get; set; }

      public string Key { get; set; }

      public List<ProductGoalDto> Goals { get; set; }

      public List<ProductItemDto> Items { get; set; }

      public List<TaskDto> Tasks { get; set; }
      public List<MilestoneDto> Milestones { get; set; }

      //public List<FileAttachment> FileAttachments { get; set; }

      //public List<UrlLink> UrlLinks { get; set; }

      //public List<Release> Releases { get; set; }
   }

   public class ProductPatchDto
   {
      public int Id { get; set; }

      public List<Int32AttributeValueDto> Int32AttributeValues { get; set; }

      //public StringAttributeValueDto[] StringAttributeValues { get; set; }

      public List<KeyPerformanceIndicatorDto> KeyPerformanceIndicators { get; set; }

      //public DecimalAttributeValueDto[] DecimalAttributeValues { get; set; }

      //public UtcDateTimeAttributeValueDto[] UtcDateTimeAttributeValues { get; set; }

      //public MoneyAttributeValueDto[] MoneyAttributeValues { get; set; }

      public List<EnumAttributeValueDto> EnumAttributeValues { get; set; }

      [MaxLength(ModelConstraints.NAME_LENGTH_300)]
      public string Name { get; set; }

      public string Description { get; set; }

      //public int? StatusId { get; set; }

      //public Status Status { get; set; }

      public string Key { get; set; }

      public List<ProductGoalDto> Goals { get; set; }

      public List<ProductItemDto> Items { get; set; }

      public List<TaskDto> Tasks { get; set; }

      //public List<FileAttachment> FileAttachments { get; set; }
   }

   public class ProductPostDto
   {
      [Required()]
      [MaxLength(ModelConstraints.NAME_LENGTH_300)]
      public string Name { get; set; }
      //public List<Int32AttributeValueDto> Int32AttributeValues { get; set; }
      public List<EnumCollectionAttributeValueDto> EnumCollectionAttributeValues { get; set; }
   }
}
