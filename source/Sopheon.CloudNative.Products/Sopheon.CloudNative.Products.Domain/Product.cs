

using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.Domain
{

    public class Product : IAllAttributesContainer
    {
        public int Id { get; set; }

        public List<Int32AttributeValue> Int32AttributeValues { get; set; }

        public List<StringAttributeValue> StringAttributeValues { get; set; }

        public List<DecimalAttributeValue> DecimalAttributeValues { get; set; }

        public List<UtcDateTimeAttributeValue> UtcDateTimeAttributeValues { get; set; }

        public List<MoneyAttributeValue> MoneyAttributeValues { get; set; }

        public string Name { get; set; }

      // TODO - confirm nullable DB column, remove entity config if no longer needed
      public string Description { get; set; }

        //public int? StatusId { get; set; }

        //public Status Status { get; set; }

        public string Key { get; set; }

        public List<Goal> Goals { get; set; }

        public List<KeyPerformanceIndicator> KeyPerformanceIndicators { get; set; }

        public List<ProductItem> Items { get; set; }

        public List<FileAttachment> FileAttachments { get; set; }

        public List<UrlLink> UrlLinks { get; set; }

        public List<Release> Releases { get; set; }
    }

    public class ProductImage
    {
        public int ProductId { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        public Product Product { get; set; }

        // Image Url in Blob Storage?
    }

    public class Release
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class FileAttachment
    {
        public int FileAttachmentId { get; set; }

        public string Name { get; set; }
    }

    public class UrlLink
    {
        public int UrlLinkId { get; set; }

        public string Name { get; set; }
    }
}
