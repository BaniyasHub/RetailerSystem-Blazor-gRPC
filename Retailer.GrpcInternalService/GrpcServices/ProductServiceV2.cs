using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;
using Grpc.Core;
using Retailer.Interface.Interfaces.Managers;
using Product.V2;

namespace Retailer.GrpcInternalService.GrpcServices
{
    public class ProductServiceV2 : Product.V2.Product.ProductBase
    {
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;
        private readonly int? tempInt = 15;

        public ProductServiceV2(IProductManager productManager, IMapper mapper)
        {
            _productManager = productManager;
            _mapper = mapper;
        }


        public async override Task BiDirectionalExampleMethod(IAsyncStreamReader<BiDirectionalRequest> requestStream, IServerStreamWriter<Product.V2.BiDirectionalResponse> responseStream, ServerCallContext context)
        {
            //It will last until getting all incoming request
            var requestTask = Task.Run(async () =>
            {
                await foreach (var requestMessage in requestStream.ReadAllAsync())
                {
                    Console.WriteLine("Request is taken");
                }
            });

            //It will lasts while requests are coming
            while (!requestTask.IsCompleted)
            {
                await responseStream.WriteAsync(new BiDirectionalResponse());
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine("Response is given");
            }

            //It will last until cancellation requested via token
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }

        public async override Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productDtoList = await _productManager.GetAllProducts();

            var productModelList = _mapper.Map<List<ProductModel>>(productDtoList);

            foreach (var productModel in productModelList)
            {
                await responseStream.WriteAsync(productModel);
            }
        }

        public async override Task<ProductModel> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            var product = await _productManager.GetProduct(request.Id);

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public async override Task<ProductModelList> GetProductsByIds(IAsyncStreamReader<GetProductByIdRequest> requestStream, ServerCallContext context)
        {
            var productIdList = new List<int>();

            while (await requestStream.MoveNext())
            {
                productIdList.Add(requestStream.Current.Id);
            }

            ProductModelList productModelList = new ProductModelList();

            productModelList.ProductModel.AddRange(_mapper.Map<List<ProductModel>>(await _productManager.GetProductsByIds(productIdList)));

            return productModelList;
        }

        public async override Task GetStreamProductsByIds(IAsyncStreamReader<GetProductByIdRequest> requestStream, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            //var userAgent = context.RequestHeaders.GetValue("user-agent");
            var productIdList = new List<int>();

            await foreach (var item in requestStream.ReadAllAsync())
            {
                productIdList.Add(item.Id);
            }

            var productModelList = _mapper.Map<List<ProductModel>>(await _productManager.GetProductsByIds(productIdList));

            foreach (var productModel in productModelList)
            {
                await responseStream.WriteAsync(productModel);
                //responseStream.WriteAsync(productModel).GetAwaiter().GetResult();
            }
        }

        //Created for seeing proto types and which ones can be automatically converted to c# types with automapper
        public async override Task<TempProductMessage> GetWisdomModel(GetWisdomModelRequest request, ServerCallContext context)
        {
            var tempModel = new TempProductMessage();

            await Task.Run(() =>
            {
                //Dictionary
                var dictionaryModel = new Dictionary<string, string>();
                dictionaryModel.Add("Mustafa", "Suyi");
                tempModel.DictionaryModel.Add(dictionaryModel);
            });

            //ByteList
            tempModel.ByteListType = ByteString.CopyFrom(Convert.FromBase64String("heyy"));

            //Custom Model > Messages that converted from C# classes
            tempModel.CategoryModel = new CategoryModel();

            //DateTime and TimeStamp
            tempModel.TimeStampType = Timestamp.FromDateTime(DateTime.UtcNow);
            tempModel.DurationType = Duration.FromTimeSpan(TimeSpan.FromMinutes(3));

            //Casual and nullable structs
            tempModel.Int32Type = 17;
            tempModel.DoubleType = 17.25;
            tempModel.NullableInt32Type = tempInt;
            tempModel.BoolType = true;

            //Lists and string
            tempModel.StringListType.Add(new List<string>()
            {
                "Mustafa","Suyi"
            });
            tempModel.StringType = "Mustafa Suyi";

            //And and OneOf
            tempModel.AnyType = Any.Pack(new ProductModel() { Color = "Blue" });
            tempModel.RequestModel = new GetProductByIdRequest() { Id = 1 };
            tempModel.AllRequestModel = new GetAllProductsRequest();

            return tempModel;
        }
    
    }
}
