using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Command to create new comment.
    /// </summary>
    public class CreateCommentCommand : IRequest<int>
    {
        /// <summary>
        /// Model.
        /// </summary>
        public CommentDTO Model { get; set; }

        public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Mapper.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public CreateCommentCommandHandler(IMapper mapper, IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Create new comment.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>New comment Id.</returns>
            public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = _mapper.Map<Comment>(request.Model);

                _context.Comments.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}