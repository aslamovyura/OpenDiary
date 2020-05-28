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
    /// <summary>
    /// Command to delete comment.
    /// </summary>
    public class DeleteCommentCommand : IRequest
    {
        /// <summary>
        /// Comment Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Delete comment.
        /// </summary>
        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor without parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public DeleteCommentCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Delete comment from DB.
            /// </summary>
            /// <param name="request">Command to delete comment.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Value.</returns>
            public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var comment = _context.Comments.Where(c => c.Id == request.Id).SingleOrDefault();

                if(comment == null)
                {
                    throw new NotFoundException(nameof(Author), request.Id);
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}