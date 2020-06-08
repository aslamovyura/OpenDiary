using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using System.Threading.Tasks;
using System.Threading;
using Infrastructure.Persistence.Configurations;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Application context to manage entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Define application DB context.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
    // ------- Do not use lines below, when runtime migration is enable -------- //
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        /// <summary>
        /// Save changes in DB context asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configure DB on model creating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CommentConfiguration());
        }
    }
}