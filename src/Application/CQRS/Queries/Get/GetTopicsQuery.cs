using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Get
{
    public class GetTopicsQuery : IRequest<IEnumerable<TopicDTO>>
    {
        /// <summary>
        /// Define topics query.
        /// </summary>
        public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, IEnumerable<TopicDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetTopicsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get topics info.
            /// </summary>
            /// <param name="request">Info request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Number of posts DTO.</returns>
            public async Task<IEnumerable<TopicDTO>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
            {
                var entities = await _context.Topics.ToArrayAsync(cancellationToken);
                var topics = _mapper.Map<IEnumerable<TopicDTO>>(entities);

                return topics;
            }
        }
    }
}