using AutoMapper;
using Moq;
using NUnit.Framework;
using Retailer.GrpcService.Protos;
using Retailer.GrpcService.Services;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Tests.GrpcServiceTest
{
    [TestFixture]
    public class ProductGrpcServiceTests
    {
        private ProductServiceV1 _productGrpcService;
        Mock<IProductManager> _productManagerMock;


        [SetUp]
        public void SetUp()
        {
            var mapperConfiguration = CreateMapperConfiguration();
            var mapper = mapperConfiguration.CreateMapper();

            _productManagerMock = new Mock<IProductManager>();
            _productGrpcService = new ProductServiceV1(_productManagerMock.Object, mapper);
        }



        //[Test]
        //public async Task GetProductById_


        private MapperConfiguration CreateMapperConfiguration()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product.V1.ProductPriceModel, ProductPriceDto>().ReverseMap();
                cfg.CreateMap<Product.V1.ProductModel, ProductDto>().ReverseMap();
                cfg.CreateMap<Product.V1.CategoryModel, CategoryDto>().ReverseMap();
                cfg.CreateMap<CustomerModel, CustomerDto>().ReverseMap();

                cfg.CreateMap<Data.Models.Category, CategoryDto>().ReverseMap();
                cfg.CreateMap<Data.Models.Customer, CustomerDto>().ReverseMap();
                cfg.CreateMap<Data.Models.Product, ProductDto>().ReverseMap();
                cfg.CreateMap<Data.Models.ProductPrice, ProductPriceDto>().ReverseMap();
            });

            return mapper;
        }
    }
}
