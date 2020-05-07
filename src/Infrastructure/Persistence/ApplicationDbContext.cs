using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

            // TODO: runtime migrations 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many commets
                b.HasMany(e => e.Comments)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many posts
                b.HasMany(e => e.Posts)
                    .WithOne()
                    .HasForeignKey(up => up.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Post>(b =>
            {
                b.HasOne(e => e.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();

                b.HasOne(e => e.Topic)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(u => u.TopicId)
                    .IsRequired();

                b.HasMany(e => e.Comments)
                    .WithOne()
                    .HasForeignKey(up => up.PostId)
                    .IsRequired();
            });

            modelBuilder.Entity<Comment>(b =>
            {
                b.HasOne(e => e.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(e => e.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.PostId)
                    .IsRequired();
            });
        }
    }
}