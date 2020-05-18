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
    /// <summary>
    /// Query for comments of the certain author.
    /// </summary>
    public class GetCommentsByAuthorIdQuery : IRequest<ICollection<CommentDTO>>
    {
        /// <summary>
        /// Author identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Define class to get comments for the certain author.
        /// </summary>
        public class GetCommentsByAuthorIdQueryHandler : IRequestHandler<GetCommentsByAuthorIdQuery, ICollection<CommentDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetCommentsByAuthorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get comments for the certain author.
            /// </summary>
            /// <param name="request">Comments request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Collection of comments DTO.</returns>
            public async Task<ICollection<CommentDTO>> Handle(GetCommentsByAuthorIdQuery request, CancellationToken cancellationToken)
            {
                var entities = await _context.Comments
                    .Where(p => p.AuthorId == request.AuthorId)
                    .OrderByDescending(p => p.Date)
                    .ToListAsync(cancellationToken);

                var comments = _mapper.Map<ICollection<CommentDTO>>(entities);

                return comments;
            }
        }
    }
}