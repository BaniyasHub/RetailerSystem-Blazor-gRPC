using AutoMapper;
using Retailer.Interface.Dtos;
using Retailer.WebAssembly.GrpcClient.Protos;

namespace Retailer.WebAssembly.MappingProfile
{
    public class WebAssemblyMappingProfile : Profile
    {
        public WebAssemblyMappingProfile()
        {
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            CreateMap<ProductModel, ProductDto>().ReverseMap();
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
        }
    }
}
