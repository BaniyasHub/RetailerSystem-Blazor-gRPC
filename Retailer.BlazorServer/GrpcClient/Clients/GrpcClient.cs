using AutoMapper;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Product.V2;
using Retailer.BlazorServer.GrpcClient.Protos;
using Retailer.Interface.Dtos;
using System.Net;
using System.Threading.Tasks;
using FileInfo = Retailer.BlazorServer.GrpcClient.Protos.FileInfo;
using HttpVersion = System.Net.HttpVersion;

namespace Retailer.BlazorServer.GrpcClient.Clients
{
    public class GrpcClient : IGrpcClient
    {
        private readonly GrpcChannel channel;
        private readonly Customer.CustomerClient _customerClient;
        private readonly Product.V2.Product.ProductClient _productClientV2;
        private readonly IMapper _mapper;

        public GrpcClient(IMapper mapper, GrpcClientFactory grpcClientFactory)
        {
            _mapper = mapper;
            channel = GrpcChannel.ForAddress("http://localhost:7042", new GrpcChannelOptions
            {
                HttpClient = new HttpClient { DefaultRequestVersion = HttpVersion.Version20 }
            });
            _productClientV2 = grpcClientFactory.CreateClient<Product.V2.Product.ProductClient>("ProductClientV2");
            _customerClient = grpcClientFactory.CreateClient<Customer.CustomerClient>("CustomerClient");
        }

        public async Task<string> GetGreeting()
        {
            var greeterClient = new Greeter.GreeterClient(channel);
            var input = new HelloRequest();
            input.Name = "Mustafa Suyi - Senior Software Developer";
            var reply = await greeterClient.SayHelloAsync(input);

            return reply.Message;
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            var customerClient = new Customer.CustomerClient(channel);

            var input = new CustomerLookUpModel();
            input.CustomerId = id;

            var reply = await customerClient.GetCustomerInfoAsync(input);

            return _mapper.Map<CustomerDto>(reply);
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            var input = new NewCustomerRequest();
            var reply = _customerClient.GetNewCustomers(input);

            var customerList = new List<CustomerModel>();

            while (await reply.ResponseStream.MoveNext())
            {
                var currentCustomer = reply.ResponseStream.Current;

                customerList.Add(currentCustomer);
            }

            return _mapper.Map<List<CustomerDto>>(customerList);
        }

        public async Task<bool> AddCustomer(CustomerDto customer)
        {
            var customerModel = _mapper.Map<CustomerModel>(customer);
            var response = await _customerClient.AddCustomerAsync(customerModel);

            return response.IsAdded;
        }

        public async Task DeleteCustomer(int id)
        {
            var customerLookupModel = new CustomerLookUpModel { CustomerId = id };

            await _customerClient.DeleteCustomerAsync(customerLookupModel);
        }

        //Server Streaming
        public async Task<List<ProductDto>> GetAllProducts()
        {
            var productList = new List<ProductModel>();

            var reply = _productClientV2.GetAllProducts(new GetAllProductsRequest());

            while (await reply.ResponseStream.MoveNext())
            {
                productList.Add(reply.ResponseStream.Current);
            }

            return _mapper.Map<List<ProductDto>>(productList);
        }

        //Client Streaming
        public async Task<List<ProductDto>> GetProductsByIds(List<int> ids = null)
        {
            ids = ids ?? new List<int>();
            //Setting up the existing Products Ids
            if (ids == null || ids.Count < 1)
            {
                ids.AddRange(new List<int>()
                {
                    2,4
                });
            }

            //Invoking Grpc method with request stream  
            var request = _productClientV2.GetProductsByIds();

            foreach (var id in ids)
            {
                await request.RequestStream.WriteAsync(new GetProductByIdRequest() { Id = id });
            }

            await request.RequestStream.CompleteAsync();

            var productModelList = await request.ResponseAsync;
            return _mapper.Map<List<ProductDto>>(productModelList.ProductModel.ToList());
        }

        //Bi-Directional Streaming
        public async Task<List<ProductDto>> GetStreamProductByIds(List<int> ids = null)
        {
            ids = ids ?? new List<int>();
            //Setting up the existing Products Ids
            if (ids == null || ids.Count < 1)
            {
                ids.AddRange(new List<int>()
                {
                    2,4,5
                });
            }

            //Creating cancellation token for while loop
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            //Invoking Grpc method with request stream
            //var request = _productClient.GetStreamProductsByIds(deadline:DateTime.UtcNow.AddSeconds(0.01));
            var request = _productClientV2.GetStreamProductsByIds();

            foreach (var id in ids)
            {
                await request.RequestStream.WriteAsync(new GetProductByIdRequest() { Id = id });
                await Task.Delay(3000);
            }

            await request.RequestStream.CompleteAsync();

            var productModelList = new List<ProductModel>();
            while (await request.ResponseStream.MoveNext(token))
            {
                productModelList.Add(request.ResponseStream.Current);
            }

            return _mapper.Map<List<ProductDto>>(productModelList);
        }

        public async Task FileDownload()
        {
            var client = new FileGreeter.FileGreeterClient(channel);

            var response = client.FileDownload(new FileInfo { FileName = "fw", FileExtension = ".pdf" });

            while (await response.ResponseStream.MoveNext())
            {

            }
        }

        public async Task FileUpload()
        {
            var client = new FileGreeter.FileGreeterClient(channel);

            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "files");
            //using FileStream fileStream = new FileStream($"{path}/InvoicePdf.pdf", FileMode.Open);
            using FileStream fileStream = new FileStream(@"C:\Users\MUSSUYI\source\repos\Retailer\Retailer.BlazorServer\wwwroot\files\InvoicePdf.pdf", FileMode.Open);
            var bytesContent = new BytesContent()
            {
                FileSize = fileStream.Length,
                ReadedByte = 0,
                FileInfo = new FileInfo()
                {
                    FileName = Path.GetFileNameWithoutExtension(fileStream.Name),
                    FileExtension = Path.GetExtension(fileStream.Name)
                }

            };

            var response = client.FileUpload();
            byte[] buffer = new byte[2048];

            while ((bytesContent.ReadedByte = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                bytesContent.Buffer = ByteString.CopyFrom(buffer);
                await response.RequestStream.WriteAsync(bytesContent);
            }
            await response.RequestStream.CompleteAsync();

            fileStream.Close();

            await response.ResponseAsync;
        }


        public async Task<WisdomDto> GetWisdomModel()
        {
            var response = await _productClientV2.GetWisdomModelAsync(new GetWisdomModelRequest { Wish = "Howdy" });

            var result = _mapper.Map<WisdomDto>(response);

            result.TimeStampTypeCustom = response.TimeStampType.ToDateTime();
            result.DurationTypeCustom = response.DurationType.ToTimeSpan();
            result.ByteListTypeCustom = response.ByteListType.ToByteArray();

            if (response.AnyType.Is(ProductModel.Descriptor))
            {
                result.Product = _mapper.Map<ProductDto>(response.AnyType.Unpack<ProductModel>());
            }

            return result;
        }

        public async Task BiDirectionalExample()
        {
            var response = _productClientV2.BiDirectionalExampleMethod();

            for (int i = 0; i < 10; i++)
            {
                await response.RequestStream.WriteAsync(new BiDirectionalRequest());
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            await response.RequestStream.CompleteAsync();

            await foreach (var item in response.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine("Client got it");
            }

        }

       
    }
}
