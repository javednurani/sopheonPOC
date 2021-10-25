using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public interface IAttributeValueDto<TValueType>
   {
      int AttributeId { get; set; }

      AttributeDto Attribute { get; set; }

      TValueType Value { get; set; }
   }

   public class StringAttributeValueDto : IAttributeValueDto<string>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public string Value { get; set; }
   }

   public class Int32AttributeValueDto : IAttributeValueDto<int?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public int? Value { get; set; }
   }

   public class DecimalAttributeValueDto : IAttributeValueDto<decimal?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public decimal? Value { get; set; }
   }

   public class MoneyValueDto
   {
      public string CurrencyCode;
      public decimal? Value;
   }

   public interface IAttributeContainer 
   {
      List<Int32AttributeValueDto> IntAttributeValues { get; set; }

      List<StringAttributeValueDto> StringAttributeValues { get; set; }

      List<DecimalAttributeValueDto> DecimalAttributeValues { get; set; }

      List<UtcDateTimeAttributeValueDto> UtcDateTimeAttributeValues { get; set; }

      List<MoneyAttributeValueDto> MoneyAttributeValues { get; set; }
   }

   public class MoneyAttributeValueDto : IAttributeValueDto<MoneyValueDto> 
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public MoneyValueDto Value { get; set; }
   }

   public class UtcDateTimeAttributeValueDto : IAttributeValueDto<DateTime?>
   {
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
