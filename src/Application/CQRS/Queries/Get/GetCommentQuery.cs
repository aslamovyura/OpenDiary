using System;
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
    /// Request for comment.
    /// </summary>
    public class GetCommentQuery : IRequest<CommentDTO>
    {
        /// <summary>
        /// Comment Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Handler of the comment queries.
        /// </summary>
        public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, CommentDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetCommentQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get comment data.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Comment DTO.</returns>
            public async Task<CommentDTO> Handle(GetCommentQuery request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await _context.Comments.Where(p => p.Id == request.Id)
                                                  .SingleOrDefaultAsync();

                var post = _mapper.Map<CommentDTO>(entity);

                return post;
            }
        }
    }
}