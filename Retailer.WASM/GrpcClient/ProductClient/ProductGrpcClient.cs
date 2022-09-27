using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Retailer.Interface.Dtos;
using Product.V1;

namespace Retailer.WASM.GrpcClient.ProductClient
{
    public class ProductGrpcClient : IProductGrpcClient
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Product.V1.Product.ProductClient _productGrpcClient;

        public ProductGrpcClient( IMapper mapper, GrpcClientFactory grpcClientFactory, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
            _productGrpcClient = grpcClientFactory.CreateClient<Product.V1.Product.ProductClient>("ProductAuthenticated");
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productGrpcClient.GetProductByIdAsync(new GetProductByIdRequest { Id = id });

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var productList = new List<ProductModel>();

            var response = _productGrpcClient.GetAllProducts(new GetAllProductsRequest());

            while (await response.ResponseStream.MoveNext())
            {
                var product = response.ResponseStream.Current;
                product.ImageUrl = _configuration.GetSection("BaseServerUrl").Value + product.ImageUrl;
                productList.Add(product);
            }

            var productDtoList = _mapper.Map<List<ProductDto>>(productList);
            return productDtoList;
        }

    }
}



    
