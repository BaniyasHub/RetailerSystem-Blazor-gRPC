using Microsoft.EntityFrameworkCore;
using Retailer.Business.Managers;
using Retailer.Business.MappingProfiles;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.Repository;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Interfaces.Managers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RetailerDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(CoreMappingProfile));

builder.Services.AddScoped<ICustomerManager, CustomerManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddCors(x => x.AddPolicy("Retailer", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Retailer");

app.UseAuthorization();

app.MapControllers();

app.Run();
