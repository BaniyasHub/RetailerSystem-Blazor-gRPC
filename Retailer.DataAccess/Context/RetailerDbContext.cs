using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Retailer.Data.Models;
using Retailer.DataAccess.FluentConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Context
{
    public class RetailerDbContext : DbContext
    {
        public RetailerDbContext(DbContextOptions<RetailerDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ProductPrice> ProductPrice { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new ProductPriceConfig());
            modelBuilder.ApplyConfiguration(new OrderDetailConfig());
            modelBuilder.ApplyConfiguration(new OrderHeaderConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
