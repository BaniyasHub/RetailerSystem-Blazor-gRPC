using AutoMapper;
using Grpc.Net.Client;
using Retailer.Interface.Dtos;
using Retailer.WebAssembly.GrpcClient.Protos;

namespace Retailer.WebAssembly.GrpcClient.Product
{
    public class ProductGrpcClient : IProductGrpcClient
    {
        private readonly IMapper _mapper;
        private readonly Protos.Product.ProductClient client;

        public ProductGrpcClient( IMapper mapper, Protos.Product.ProductClient client)
        {
            _mapper = mapper;
            this.client = client;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = client.GetProductById(new Protos.GetProductByIdRequest { Id = id });

            var productDto = await Task.Run(() => _mapper.Map<ProductDto>(product));

            return productDto;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var response = client.GetAllProducts(new Protos.GetAllProductsRequest());

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            List<ProductModel> productModelList = new();

            while (await response.ResponseStream.MoveNext(token))
            {
                productModelList.Add(response.ResponseStream.Current);
            }

            var productDtoList = _mapper.Map<List<ProductDto>>(productModelList);

            return productDtoList;
        }
    }
}
