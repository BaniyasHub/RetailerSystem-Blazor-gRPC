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
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> modelBuilder)
        {
            modelBuilder.HasKey(x => x.Id);

            modelBuilder.HasOne(x => x.OrderHeader)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderHeaderId)
                .HasPrincipalKey(x => x.Id);

            modelBuilder.HasOne(x => x.Product)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}
