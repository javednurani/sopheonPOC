using AutoMapper;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.AspNetCore.MappingProfiles
{

   public class ProductProfile : Profile
   {
      public ProductProfile()
      {
         CreateMap<Attribute, AttributeDto>().ReverseMap();
         CreateMap<DecimalAttribute, DecimalAttributeDto>().ReverseMap();
         CreateMap<AttributeDataType, AttributeDataTypeDto>().ReverseMap();

         CreateMap<Product, ProductDto>().ReverseMap();
         CreateMap<Product, ProductPatchDto>().ReverseMap();
         CreateMap<Product, ProductPostDto>().ReverseMap();

         CreateMap<Goal, ProductGoalDto>().ReverseMap();
         CreateMap<ProductItemType, ProductItemTypeDto>().ReverseMap();
         CreateMap<ProductItem, ProductItemDto>().ReverseMap();

         CreateMap<Int32AttributeValue, Int32AttributeValueDto>().ReverseMap();

         CreateMap<StringAttributeValue, StringAttributeValueDto>().ReverseMap();

         CreateMap<DecimalAttributeValue, DecimalAttributeValueDto>().ReverseMap();

         CreateMap<UtcDateTimeAttributeValue, UtcDateTimeAttributeValueDto>().ReverseMap();

         CreateMap<EnumAttributeValue, EnumAttributeValueDto>().ReverseMap();
         CreateMap<EnumCollectionAttributeValue, EnumCollectionAttributeValueDto>().ReverseMap();
         CreateMap<EnumAttributeOptionValue, EnumAttributeOptionValueDto>().ReverseMap();
         CreateMap<EnumAttributeOption, EnumAttributeOptionDto>().ReverseMap();

         CreateMap<MoneyAttributeValue, MoneyAttributeValueDto>().ReverseMap();
         CreateMap<MoneyValue, MoneyValueDto>().ReverseMap();

         CreateMap<Status, StatusDto>().ReverseMap();

         CreateMap<KeyPerformanceIndicator, KeyPerformanceIndicatorDto>().ReverseMap();
      }
   }
}
