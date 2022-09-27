using Blazored.LocalStorage;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Net.Http.Headers;
using Retailer.WASM;
using Retailer.WASM.GrpcClient.ProductClient;
using Retailer.WASM.MappingProfile;
using Retailer.WASM.Service;
using Retailer.WASM.Service.IService;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Add Custom 3rd Libraries
builder.Services.AddAutoMapper(typeof(WebAssemblyMappingProfile));
builder.Services.AddBlazoredLocalStorage();


//Adding Http Client
builder.Services.AddHttpClient("product", client =>
{
    client.BaseAddress = new Uri("https://localhost:44304/product");
});

builder.Services.AddHttpClient("order", client =>
{
    client.BaseAddress = new Uri("https://localhost:44304/order");

});

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

//Adding GrpcClient
builder.Services
    .AddGrpcClient<Customer.Customer.CustomerClient>("CustomerAuthenticated", x =>
    {
        x.Address = new Uri("http://localhost:7042");
        //x.Interceptors.Add()
    })
    .ConfigurePrimaryHttpMessageHandler(() => grpcWebHandler);


builder.Services
    .AddGrpcClient<Product.V1.Product.ProductClient>("ProductAuthenticated", x =>
        {
            x.Address = new Uri("http://localhost:7042");
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

//Building and Running the Client
await builder.Build().RunAsync();























//client.DefaultRequestHeaders.Add(
//    HeaderNames.Accept, "application/vnd.github.v3+json");
//client.DefaultRequestHeaders.Add(
//    HeaderNames.UserAgent, "HttpRequestsSample");



//var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new
//                  HttpClientHandler());



//.AddInterceptor<Interceptor>(InterceptorScope.Client)
//.ConfigurePrimaryHttpMessageHandler(() =>
//    {
//        var handler = new HttpClientHandler();
//        return handler;
//    })
//.ConfigureChannel(o =>
//    {
//        //o.HttpHandler
//    })
// .AddCallCredentials((context, metadata) =>
//     {
//         if (!string.IsNullOrEmpty("token"))
//         {
//             metadata.Add("Authorization", $"Bearer token");
//         }
//         return Task.CompletedTask;
//     });



//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
