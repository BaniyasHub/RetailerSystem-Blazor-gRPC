using AutoMapper;
using Retailer.Data.Models;
using Retailer.Interface.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Tests.AutoMapper
{
    public class MappingProfileForTests : Profile
    {
        public MappingProfileForTests()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
