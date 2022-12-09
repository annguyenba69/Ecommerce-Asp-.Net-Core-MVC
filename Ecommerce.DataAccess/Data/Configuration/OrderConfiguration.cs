using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.HasOne(o => o.PaymentMethod)
                .WithMany(pm => pm.Orders)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.OrderStatus)
                .WithMany(pm => pm.Orders)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.ApplicationUser)
                .WithMany(ap => ap.Orders)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
