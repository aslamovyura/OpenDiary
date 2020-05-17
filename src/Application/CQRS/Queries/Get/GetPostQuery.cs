using System;
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
    /// <summary>
    /// Defice class to get post info.
    /// </summary>
    public class GetPostQuery : IRequest<PostDTO>
    {
        /// <summary>
        /// Post Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Handler of the post queries.
        /// </summary>
        public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetPostQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get post data.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Post DTO.</returns>
            public async Task<PostDTO> Handle(GetPostQuery request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await GetPost(request);

                var post = _mapper.Map<PostDTO>(entity);

                return post;
            }

            private async Task<Post> GetPost(GetPostQuery request)
            {
                return await _context.Posts.Where(p => p.Id == request.Id)
                                                  .SingleOrDefaultAsync();

            }
        }
    }
}