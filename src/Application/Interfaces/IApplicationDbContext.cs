using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for the application context.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// User posts.
        /// </summary>
        DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Topics of the user posts.
        /// </summary>
        DbSet<Topic> Topics { get; set; }

        /// <summary>
        /// Comments to user posts.
        /// </summary>
        DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Save data asynchronously (override).
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Savint results.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}