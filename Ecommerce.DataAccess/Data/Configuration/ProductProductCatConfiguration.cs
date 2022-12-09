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
    public class ProductProductCatConfiguration : IEntityTypeConfiguration<ProductProductCat>
    {
        public void Configure(EntityTypeBuilder<ProductProductCat> builder)
        {


            builder.HasKey(ppc => new { ppc.ProductId, ppc.ProductCatId });

            builder.HasOne(ppc => ppc.ProductCat)
                .WithMany(pc => pc.ProductProductCats)
                .HasForeignKey(ppc => ppc.ProductCatId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(ppc => ppc.Product)
                .WithMany(p => p.ProductProductCats)
                .HasForeignKey(ppc => ppc.ProductId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
