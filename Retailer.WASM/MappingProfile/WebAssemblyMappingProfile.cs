using AutoMapper;
using Customer;
using Product.V1;
using Retailer.Interface.Dtos;
using Retailer.WASM.GrpcClient;

namespace Retailer.WASM.MappingProfile
{
    public class WebAssemblyMappingProfile : Profile
    {
        public WebAssemblyMappingProfile()
        {
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            CreateMap<ProductModel, ProductDto>().ReverseMap();
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
            CreateMap<ProductPriceModel, ProductPriceDto>().ReverseMap();
        }
    }
}
