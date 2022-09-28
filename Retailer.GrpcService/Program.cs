using Retailer.GrpcService.MappingProfie;
using Retailer.GrpcService.Services;
using AutoMapper;
using Retailer.Interface.Interfaces.Managers;
using Retailer.Business.Managers;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.DataAccess.Repository;
using Retailer.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(options => {
    options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
    options.EnableDetailedErrors = true;
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));



builder.Services.AddDbContext<RetailerDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(GrpcMappingProfile));

builder.Services.AddScoped<ICustomerManager, CustomerManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();


var app = builder.Build();



//Configure the HTTP request pipeline.
app.UseRouting();


app.UseGrpcWeb(new GrpcWebOptions
{
    DefaultEnabled = true
});

app.UseCors();

app.MapGrpcService<GreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
app.MapGrpcService<CustomerService>().EnableGrpcWeb().RequireCors("AllowAll");
app.MapGrpcService<ProductServiceV1>().EnableGrpcWeb().RequireCors("AllowAll");

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
