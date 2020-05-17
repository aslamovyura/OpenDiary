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
    public class GetAuthorsQuery : IRequest<IEnumerable<AuthorDTO>>
    {
        /// <summary>
        /// Define class to get info on the whole posts in app.
        /// </summary>
        public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Automapper.</param>
            public GetAuthorsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get authors info.
            /// </summary>
            /// <param name="request">Info request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Number of authors DTO.</returns>
            public async Task<IEnumerable<AuthorDTO>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
            {
                var entites = await _context.Authors.ToArrayAsync(cancellationToken);
                var authors = _mapper.Map<IEnumerable<AuthorDTO>>(entites);

                return authors;
            }
        }
    }
}