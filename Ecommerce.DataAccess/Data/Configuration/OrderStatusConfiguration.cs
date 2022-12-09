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
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasKey(os => os.Id);
            builder.Property(os => os.Id).UseIdentityColumn(1,1).ValueGeneratedOnAdd();
            builder.HasMany(os => os.Orders)
                .WithOne(o => o.OrderStatus)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
