using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Retailer.Business.Managers;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.DataAccess.Repository;
using Retailer.GrpcInternalService.GrpcServices;
using Retailer.GrpcInternalService.MappingProfiles;
using Retailer.GrpcInternalService.Services;
using Retailer.Interface.Interfaces.Managers;
using Retailer.GrpcInternalService.Utility;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(x =>
{
    x.EnableDetailedErrors = true;
    x.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    x.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
});


builder.Services.AddDbContext<RetailerDbContext>(x => 
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddPersistenceServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductServiceV2>();
app.MapGrpcService<CustomerService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
