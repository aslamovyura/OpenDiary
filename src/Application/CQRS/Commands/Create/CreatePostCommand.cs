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
    /// Create new post.
    /// </summary>
    public class CreatePostCommand : IRequest<int>
    {
        /// <summary>
        /// Post DTO.
        /// </summary>
        public PostDTO Model { get; set; }

        public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Mapper.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public CreatePostCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Create new post.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>New post Id.</returns>
            public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = _mapper.Map<Post>(request.Model);

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}