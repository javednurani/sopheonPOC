using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.Domain
{
   public interface IAttributeValue<TValueType>
   {
      int AttributeId { get; set; }

      Attribute Attribute { get; set; }

      TValueType Value { get; set; }
   }

   public class StringAttributeValue : IAttributeValue<string>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public string Value { get; set; }
   }

   public class Int32AttributeValue : IAttributeValue<int?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public int? Value { get; set; }
   }

   public class DecimalAttributeValue : IAttributeValue<decimal?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public decimal? Value { get; set; }
   }

   public class MoneyValue
   {
      public string CurrencyCode;
      public decimal? Value;
   }

   public interface IAttributeContainer
   {
      List<Int32AttributeValue> IntAttributeValues { get; set; }

      List<StringAttributeValue> StringAttributeValues { get; set; }

      List<DecimalAttributeValue> DecimalAttributeValues { get; set; }

      List<UtcDateTimeAttributeValue> UtcDateTimeAttributeValues { get; set; }

      List<MoneyAttributeValue> MoneyAttributeValues { get; set; }
   }

   public class MoneyAttributeValue : IAttributeValue<MoneyValue>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public MoneyValue Value { get; set; }
   }

   public class UtcDateTimeAttributeValue : IAttributeValue<DateTime?>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
