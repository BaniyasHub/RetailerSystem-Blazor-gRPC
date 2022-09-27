using AutoMapper;
using Retailer.GrpcInternalService.Protos;
using Retailer.Interface.Dtos;

namespace Retailer.GrpcInternalService.MappingProfiles
{
    public class GrpcInternalServiceMappingProfile :Profile
    {
        public GrpcInternalServiceMappingProfile()
        {
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            CreateMap<Product.V2.ProductModel, ProductDto>().ReverseMap();
            CreateMap<Product.V2.CategoryModel, CategoryDto>().ReverseMap();
            CreateMap<Data.Models.Category, CategoryDto>().ReverseMap();
            CreateMap<Data.Models.Customer, CustomerDto>().ReverseMap();
            CreateMap<Data.Models.Product, ProductDto>().ReverseMap();
            CreateMap<Data.Models.ProductPrice, ProductPriceDto>().ReverseMap();
        }
    }
}
