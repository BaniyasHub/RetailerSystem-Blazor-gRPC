using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Retailer.WebAssembly;
using Retailer.WebAssembly.GrpcClient.Product;
using Retailer.WebAssembly.GrpcClient.Protos;
using Retailer.WebAssembly.MappingProfile;
using Retailer.WebAssembly.Service;
using Retailer.WebAssembly.Service.IService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddAutoMapper(typeof(WebAssemblyMappingProfile));

builder.Services.AddGrpcClient<Product.ProductClient>(o =>
{
    o.Address = new Uri("http://localhost:5251");
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductGrpcClient, ProductGrpcClient>();
await builder.Build().RunAsync();
