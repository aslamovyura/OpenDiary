using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Get
{
    public class GetCommentsByPostIdQuery : IRequest<ICollection<CommentDTO>>
    {
        /// <summary>
        /// Post identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Define class to get comments for the posts.
        /// </summary>
        public class GetCommentsByPostIdQueryHandler : IRequestHandler<GetCommentsByPostIdQuery, ICollection<CommentDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetCommentsByPostIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get comments for the post.
            /// </summary>
            /// <param name="request">Comments request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Collection of comments DTO.</returns>
            public async Task<ICollection<CommentDTO>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
            {
                var entities = await _context.Comments
                    .Where(p => p.PostId == request.PostId)
                    .OrderByDescending(p => p.Date)
                    .ToListAsync(cancellationToken);

                var comments = _mapper.Map<ICollection<CommentDTO>>(entities);

                return comments;
            }
        }
    }
}