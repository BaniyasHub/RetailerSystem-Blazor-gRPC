using Microsoft.EntityFrameworkCore;
using Retailer.BlazorServer.GrpcClient.Clients;
using Retailer.BlazorServer.MappingProfile;
using Retailer.BlazorServer.Service;
using Retailer.BlazorServer.Service.IService;
using Retailer.Business.Managers;
using Retailer.Business.MappingProfiles;
using Retailer.Common.Utility;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.DapperDemo;
using Retailer.DataAccess.Repository;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Interfaces.Managers;


namespace Retailer.BlazorServer.Utility
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            var assemblies = AutoMapperConfiguration.GetAutoMapperProfilesFromAllAssemblies().Distinct().ToList();
            services.AddAutoMapper(typeof(CoreMappingProfile));
            services.AddAutoMapper(typeof(ClientMappingProfile));
            //services.AddAutoMapper(typeof(MappingProfileForTests));
            services.AddAutoMapper(assemblies.ToArray());

            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IGrpcClient, GrpcClient.Clients.GrpcClient>();


            #region LearnBlazor
            services.AddScoped<IPeopleData, PeopleData>();
            services.AddScoped<ISqlDataAccess, SqlDataAccess>();

            #endregion

        }
    }
}
