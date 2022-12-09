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
    public class ProductCatConfiguration : IEntityTypeConfiguration<ProductCat>
    {
        public void Configure(EntityTypeBuilder<ProductCat> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1,1).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.HasOne(a => a.ApplicationUser)
                .WithMany(pc => pc.ProductCats);
        }
    }
}
