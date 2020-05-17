using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Delete
{
    public class DeletePostCommand : IRequest
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Delete post.
        /// </summary>
        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor without parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            public DeletePostCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Delete post from DB.
            /// </summary>
            /// <param name="request">Delete command.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Value.</returns>
            public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var post = _context.Posts.Where(p => p.Id == request.Id).SingleOrDefault();

                if(post == null)
                {
                    throw new NotFoundException(nameof(Post), request.Id);
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}