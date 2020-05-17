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
    /// Update post.
    /// </summary>
    public class UpdatePostCommand : IRequest
    {
        /// <summary>
        /// Post for edit.
        /// </summary>
        public PostDTO Model { get; set; }

        /// <summary>
        /// Update post.
        /// </summary>
        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            public UpdatePostCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Update post.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns>Void value.</returns>
            public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var post = _context.Posts.Where(p => p.Id == request.Model.Id)
                                                .SingleOrDefault();

                if (post == null)
                {
                    throw new NotFoundException(nameof(Post));
                }

                post.Title = request.Model.Title;
                post.Text = request.Model.Text;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}