using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Author> Authors { get; set; }
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


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comment>(b =>
            {
                b.HasOne(e => e.Author)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.AuthorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(e => e.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(u => u.PostId)
                    .IsRequired();
            });

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ApplicationUser>(b =>
        //    {
        //        // Each User can have many commets
        //        b.HasMany(e => e.Comments)
        //            .WithOne()
        //            .HasForeignKey(uc => uc.UserId)
        //            .IsRequired();

        //        // Each User can have many posts
        //        b.HasMany(e => e.Posts)
        //            .WithOne()
        //            .HasForeignKey(up => up.UserId)
        //            .IsRequired();
        //    });

        //    modelBuilder.Entity<Post>(b =>
        //    {
        //        b.HasOne(e => e.User)
        //            .WithMany(p => p.Posts)
        //            .HasForeignKey(u => u.UserId)
        //            .IsRequired();

        //        b.HasOne(e => e.Topic)
        //            .WithMany(p => p.Posts)
        //            .HasForeignKey(u => u.TopicId)
        //            .IsRequired();

        //        b.HasMany(e => e.Comments)
        //            .WithOne()
        //            .HasForeignKey(up => up.PostId)
        //            .IsRequired();
        //    });

        //    modelBuilder.Entity<Comment>(b =>
        //    {
        //        b.HasOne(e => e.User)
        //            .WithMany(p => p.Comments)
        //            .HasForeignKey(u => u.UserId)
        //            .IsRequired()
        //            .OnDelete(DeleteBehavior.NoAction);

        //        b.HasOne(e => e.Post)
        //            .WithMany(p => p.Comments)
        //            .HasForeignKey(u => u.PostId)
        //            .IsRequired();
        //    });
        //}
    }
}