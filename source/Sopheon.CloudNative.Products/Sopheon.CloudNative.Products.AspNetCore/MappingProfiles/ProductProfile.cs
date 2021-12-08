using AutoMapper;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.MappingProfiles
{

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Attribute, AttributeDto>().ReverseMap();
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

            CreateMap<MoneyAttributeValue, MoneyAttributeValueDto>().ReverseMap();
            CreateMap<MoneyValue, MoneyValueDto>().ReverseMap();

            CreateMap<Status, StatusDto>().ReverseMap();

            CreateMap<KeyPerformanceIndicator, KeyPerformanceIndicatorDto>().ReverseMap();
        }
    }
}
