using Blazored.LocalStorage;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Retailer.WASM;
using Retailer.WASM.ClientInterceptors;
using Retailer.WASM.GrpcClient.ProductClient;
using Retailer.WASM.MappingProfile;
using Retailer.WASM.Service;
using Retailer.WASM.Service.IService;
using System.Net;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Add Custom 3rd Libraries
builder.Services.AddAutoMapper(typeof(WebAssemblyMappingProfile));
builder.Services.AddBlazoredLocalStorage();


//Adding Http Clients
builder.Services.AddHttpClient("product", client =>
{
    client.BaseAddress = new Uri("https://localhost:44304/product");
});

builder.Services.AddHttpClient("order", client =>
{
    client.BaseAddress = new Uri("https://localhost:44304/order");

});


//Creating configurations for GrpcClients
var retryMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },//Can be configured
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};

var hedgingMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    HedgingPolicy = new HedgingPolicy
    {
        MaxAttempts = 5,
        NonFatalStatusCodes = { StatusCode.Unavailable }
    }
};

var grpcWebHandler = new GrpcWebHandler(new HttpClientHandler()
{
    //MaxConnectionsPerServer = 200,
    //AllowAutoRedirect = true,
    //UseProxy = true,    
    //UseCookies = true,  
    //UseDefaultCredentials = true,
});
//grpcWebHandler.HttpVersion = HttpVersion.Version11; Works in both version, why?
grpcWebHandler.HttpVersion = HttpVersion.Version20;


//Adding GrpcClient
builder.Services
    .AddGrpcClient<Customer.Customer.CustomerClient>("CustomerClient", x =>
        {
            x.Address = new Uri("http://localhost:7042");
            //x.Interceptors.Add()
        })
    .ConfigurePrimaryHttpMessageHandler(() => grpcWebHandler);


builder.Services
    .AddGrpcClient<Product.V1.Product.ProductClient>("ProductClient", x =>
        {
            x.Address = new Uri("http://localhost:7042");
            x.Interceptors.Add(new ClientLoggingInterceptor(new LoggerFactory()));
        })
    .ConfigureChannel(x =>
        {
            x.HttpHandler = grpcWebHandler;
            x.ServiceConfig = new ServiceConfig()
            {
                MethodConfigs =
                {
                    retryMethodConfig
                }
            };
        });


//Adding business services 
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductGrpcClient, ProductGrpcClient>();
builder.Services.AddScoped<ICartService, CartService>();


//Adding Memory Config
var retailerData = new Dictionary<string, string>()
{
    { "color", "blue" },
    { "type", "car" },
    { "retailers:count", "3" },
    { "retailers:brand", "Blazin" },
    { "retailers:brand:type", "rally" },
    { "retailers:year", "2008" },
};
var memoryConfig = new MemoryConfigurationSource { InitialData = retailerData };
builder.Configuration.Add(memoryConfig);


//Building and Running the WASM
//await builder.Build().RunAsync();

//We can create a some process behaves like a pipeline by seperating commented method above
var host = builder.Build();



await host.RunAsync();
















