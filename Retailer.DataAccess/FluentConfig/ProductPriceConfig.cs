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
    public class ProductPriceConfig : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> modelBuilder)
        {
            modelBuilder.HasKey(x => x.Id);

            modelBuilder.HasOne(x => x.Product)
                .WithMany(x => x.ProductPriceList)
                .HasForeignKey(x => x.ProductId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}
