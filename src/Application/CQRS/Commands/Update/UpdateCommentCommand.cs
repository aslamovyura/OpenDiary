using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Update comment.
    /// </summary>
    public class UpdateCommentCommand : IRequest
    {
        /// <summary>
        /// Comment Data Transfer Object (DTO).
        /// </summary>
        public CommentDTO Model { get; set; }

        /// <summary>
        /// Update comment.
        /// </summary>
        public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public UpdateCommentCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Update comment.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns>Void value.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var comment = _context.Comments.Where(a => a.Id == request.Model.Id).SingleOrDefault();
                if (comment == null)
                {
                    throw new NotFoundException(nameof(Comment), request.Model.Id);
                }

                comment.Text = request.Model.Text;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}