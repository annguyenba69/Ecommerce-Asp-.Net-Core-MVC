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
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(pm => pm.Id);
            builder.Property(pm => pm.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.HasMany(pm => pm.Orders)
                .WithOne(o => o.PaymentMethod)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
