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
    public class PostCatConfiguration : IEntityTypeConfiguration<PostCat>
    {
        public void Configure(EntityTypeBuilder<PostCat> builder)
        {
            builder.HasKey(pc => pc.Id);
            builder.Property(pc => pc.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.Property(pc => pc.Name).HasMaxLength(50);
            builder.HasMany(p => p.Posts)
                .WithOne(p => p.PostCat)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(p =>p.ApplicationUser)
                .WithMany(a=> a.PostCats)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
