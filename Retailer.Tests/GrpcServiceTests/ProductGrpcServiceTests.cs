using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Product.V1;
using Retailer.GrpcService.Protos;
using Retailer.GrpcService.Services;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using Retailer.Tests.Helpers;

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

        [Test]
        public async Task GetProductById_IdGreaterThanZero_ReturnProduct()
        {
            var httpContext = new DefaultHttpContext();
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            _productManagerMock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(GetProductDto());

            var response = await _productGrpcService.GetProductById(new GetProductByIdRequest() { Id = 2 }, serverCallContext);

            _productManagerMock.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Once);

            Assert.That(response.Name, Is.EqualTo("Cochlear"));
        }

        [Test]
        public void GetProductById_IdNotGreaterThanZero_ThrowException()
        {
            var httpContext = new DefaultHttpContext();
            var serverCallContext = TestServerCallContext.Create();
            serverCallContext.UserState["__HttpContext"] = httpContext;

            _productManagerMock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(GetProductDto());

            Assert.That(async () => await _productGrpcService.GetProductById(new GetProductByIdRequest() { Id = 0 }, serverCallContext),
                Throws.ArgumentException);

            _productManagerMock.Verify(x => x.GetProduct(It.IsAny<int>()), Times.Never);
        }

        private async Task<ProductDto> GetProductDto()
        {
            var product = new ProductDto();
            var task = Task.Run(() =>
            {
                product.Id = 2;
                product.CategoryId = 2;
                product.Description = "temp";
                product.ImageUrl = "temp";
                product.Color = "Yellow";
                product.Name = "Cochlear";
            });

            await task;

            return product;
        }

        private MapperConfiguration CreateMapperConfiguration()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDto>().ReverseMap()
                    .ForMember(x => x.Category, y => y.Ignore())
                    .ForMember(x => x.ProductPriceList, y => y.Ignore());

                cfg.CreateMap<ProductPriceDto, ProductModel>().ReverseMap();

                cfg.CreateMap<CategoryModel, CategoryDto>().ReverseMap();

            });

            return mapper;
        }
    }
}
