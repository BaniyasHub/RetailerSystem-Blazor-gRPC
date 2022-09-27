using AutoMapper;
using Retailer.Data.Models;
using Retailer.Interface.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Business.MappingProfiles
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap()
                //.ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<ProductPrice, ProductPriceDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();

            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();



        }
    }
}
