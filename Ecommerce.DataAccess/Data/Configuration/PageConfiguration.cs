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
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.HasOne(p => p.ApplicationUser)
                .WithMany(a => a.Pages)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
