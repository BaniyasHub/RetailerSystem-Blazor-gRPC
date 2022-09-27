using AutoMapper;
using Retailer.Interface.Dtos;
using Retailer.GrpcService.Protos;

namespace Retailer.GrpcService.MappingProfie
{
    public class GrpcMappingProfile : Profile
    {
        public GrpcMappingProfile()
        {
            CreateMap<Product.V1.ProductPriceModel, ProductPriceDto>().ReverseMap();
            CreateMap<Product.V1.ProductModel, ProductDto>().ReverseMap();
            CreateMap<Product.V1.CategoryModel, CategoryDto>().ReverseMap();
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();

            CreateMap<Data.Models.Category, CategoryDto>().ReverseMap();
            CreateMap<Data.Models.Customer, CustomerDto>().ReverseMap();
            CreateMap<Data.Models.Product, ProductDto>().ReverseMap();
            CreateMap<Data.Models.ProductPrice, ProductPriceDto>().ReverseMap();
        }
    }
}
