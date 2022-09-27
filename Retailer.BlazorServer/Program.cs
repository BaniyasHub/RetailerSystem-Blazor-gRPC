using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Retailer.Business.Managers;
using Retailer.Business.MappingProfiles;
using Retailer.Common.Utility;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.DapperDemo;
using Retailer.DataAccess.Repository;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Interfaces.Managers;
using Retailer.BlazorServer.Utility;
using Microsoft.AspNetCore.Identity;
using Retailer.BlazorServer.GrpcClient.Protos;
using Grpc.Core.Interceptors;
using Grpc.Net.ClientFactory;
using System.Net;
using NuGet.Protocol;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<RetailerDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging(true));

builder.Services.AddPersistenceServices();


builder.Services.AddGrpcClient<Customer.CustomerClient>("CustomerClient", o =>
{
    o.Address = new Uri("http://localhost:5026");
});

builder.Services.AddGrpcClient<Product.V2.Product.ProductClient>("ProductClientV2", o =>
{
    o.Address = new Uri("http://localhost:5026");
}); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/BuiltIn/_Host");

AppDbInitializer.Seed(app);

app.Run();


//Error Handling
//AOT for WASM
//Call components from JS
//Upload file size increased to 2GB
//Accessibility Improvements
//Manipulate the Query String
