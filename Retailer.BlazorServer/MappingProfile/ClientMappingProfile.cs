using AutoMapper;
using Product.V2;
using Retailer.BlazorServer.GrpcClient;
using Retailer.BlazorServer.GrpcClient.Protos;
using Retailer.Interface.Dtos;

namespace Retailer.BlazorServer.MappingProfile
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<ProductModel, ProductDto>().ReverseMap();
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
            CreateMap<WisdomDto, TempProductMessage>().ReverseMap();
                //.ForMember(x => x.DurationType, y => y.Ignore())
                //.ForMember(x => x.TimeStampType, y => y.Ignore())
                //.ForMember(x => x.ByteListType, y => y.Ignore());
        }
    }
}
