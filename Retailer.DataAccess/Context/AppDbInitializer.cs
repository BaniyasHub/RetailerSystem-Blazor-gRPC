using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Context
{
    public class AppDbInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RetailerDbContext>();

                if (context.Category.Count() < 5 && context.Category.FirstOrDefault(x => x.Name == "Seeded") == null)
                {
                    await context.Category.AddAsync(new Category
                    {
                        Name = "Seeded"
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
