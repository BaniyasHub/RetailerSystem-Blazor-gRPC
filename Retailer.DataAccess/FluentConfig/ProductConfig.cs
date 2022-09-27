using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.FluentConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.HasKey(x => x.Id);

            modelBuilder.HasOne(x => x.Category)
                .WithMany(x => x.ProductList)
                .HasForeignKey(x => x.CategoryId)
                .HasPrincipalKey(x => x.Id);

        }
    }
}
