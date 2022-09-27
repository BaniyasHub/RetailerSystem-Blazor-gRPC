using Retailer.Business.Managers;
using Retailer.Business.MappingProfiles;
using Retailer.Common.Utility;
using Retailer.DataAccess.DapperDemo;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.DataAccess.Repository;
using Retailer.Interface.Interfaces.Managers;
using Retailer.DataAccess.Context;
using Retailer.GrpcInternalService.MappingProfiles;
using Microsoft.EntityFrameworkCore;

namespace Retailer.GrpcInternalService.Utility
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            var assemblies = AutoMapperConfiguration.GetAutoMapperProfilesFromAllAssemblies().Distinct().ToList();
            services.AddAutoMapper(typeof(CoreMappingProfile));
            services.AddAutoMapper(typeof(GrpcInternalServiceMappingProfile));
            services.AddAutoMapper(assemblies.ToArray());

            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

        }
    }
}
