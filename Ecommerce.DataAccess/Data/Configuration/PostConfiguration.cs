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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.Property(p => p.Title).HasMaxLength(100);
            builder.HasOne(p => p.PostCat)
                .WithMany(pc => pc.Posts)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(p => p.ApplicationUser)
                .WithMany(a => a.Posts)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
