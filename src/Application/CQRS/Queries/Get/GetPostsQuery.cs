using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Get
{
    public class GetPostsQuery : IRequest<IEnumerable<PostDTO>>
    {
        /// <summary>
        /// Define class to get info on the whole posts in app.
        /// </summary>
        public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetPostsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get posts info.
            /// </summary>
            /// <param name="request">Info request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Number of posts DTO.</returns>
            public async Task<IEnumerable<PostDTO>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
            {
                var entities = await _context.Posts.ToArrayAsync(cancellationToken);
                var posts = _mapper.Map<IEnumerable<PostDTO>>(entities);

                return posts;
            }
        }
    }
}