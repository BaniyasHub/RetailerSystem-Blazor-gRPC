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

        public ProductGrpcClient(IMapper mapper, GrpcClientFactory grpcClientFactory, IConfiguration configuration)
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

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            //Deadline will be valid on all retries, also if we are calling a server which calls another Grpc Server >>
            //>>EnableCallContextPropagation() method can be used when registering Client
            var response = _productGrpcClient.GetAllProducts(new GetAllProductsRequest(), cancellationToken: token, deadline: DateTime.UtcNow.AddSeconds(10));
            try
            {
                //We can run background tasks in client while processing response messages
                //var task2 = Task.Run(async () =>
                //{
                //    for (int i = 0; i < 50; i++)
                //    {
                //        productList.Add(new ProductModel());
                //       await Task.Delay(2000);
                //    }
                //});

                var task1 = Task.Run(async () =>
                {
                    while (await response.ResponseStream.MoveNext())
                    {
                        var product = response.ResponseStream.Current;

                        //Along with the situations like deadline and retry exceptions we can raise the Token manually to inform the server to abort the call ASAP
                        //if (product.Name == "Mustafa")
                        //{
                        //    cts.Cancel();
                        //}

                        product.ImageUrl = _configuration.GetSection("BaseServerUrl").Value + product.ImageUrl;
                        productList.Add(product);
                    }
                });


                await task1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return new List<ProductDto>();
            }

            var productDtoList = _mapper.Map<List<ProductDto>>(productList);
            return productDtoList;
        }


        public async Task<List<ProductDto>> GetProductsByIds(List<int> productIds)
        {
            var request = new GetProductsByIdsServerStreamingRequest();
            request.ProductIdList.AddRange(productIds);

            var productList = new List<ProductDto>();
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var response = _productGrpcClient.GetProductsByIdsServerStreaming(request, deadline: DateTime.UtcNow.AddSeconds(15), cancellationToken: token);

            await foreach (var productModel in response.ResponseStream.ReadAllAsync())
            {
                productModel.ImageUrl = _configuration.GetSection("BaseServerUrl").Value + productModel.ImageUrl;
                productList.Add(_mapper.Map<ProductDto>(productModel));

                //Just for convenience
                if (productModel.Id == 553634)
                {
                    cts.Cancel();
                }
            }


            return productList;
        }

    }
}




