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
    public class DeleteAuthorCommand : IRequest
    {
        /// <summary>
        /// Author Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Delete author.
        /// </summary>
        public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor without parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public DeleteAuthorCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Delete author from DB.
            /// </summary>
            /// <param name="request">Command to delete author.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Value.</returns>
            public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var author = _context.Authors.Where(a => a.Id == request.Id).SingleOrDefault();

                if(author == null)
                {
                    throw new NotFoundException(nameof(Author), request.Id);
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}