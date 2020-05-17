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
    /// Defice topic query.
    /// </summary>
    public class GetTopicQuery : IRequest<TopicDTO>
    {
        /// <summary>
        /// Topic Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Handler of the topic queries.
        /// </summary>
        public class GetTopicQueryHandler : IRequestHandler<GetTopicQuery, TopicDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetTopicQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get topic data.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Post DTO.</returns>
            public async Task<TopicDTO> Handle(GetTopicQuery request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await _context.Topics.Where(p => p.Id == request.Id).SingleOrDefaultAsync();
                var post = _mapper.Map<TopicDTO>(entity);

                return post;
            }
        }
    }
}