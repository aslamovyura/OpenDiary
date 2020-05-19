using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(e => e.Author)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.AuthorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.PostId)
                    .IsRequired();
        }
    }
}
