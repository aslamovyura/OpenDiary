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
    public class GetAuthorByUserIdQuery : IRequest<AuthorDTO>
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Handler of the author queries.
        /// </summary>
        public class GetAuthorByUserIdQueryHandler : IRequestHandler<GetAuthorByUserIdQuery, AuthorDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetAuthorByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Get data for authors.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Collection of authors DTO.</returns>
            public async Task<AuthorDTO> Handle(GetAuthorByUserIdQuery request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await _context.Authors.Where(a => a.UserId == request.UserId).SingleOrDefaultAsync();
                var author = _mapper.Map<AuthorDTO>(entity);

                return author;
            }
        }
    }
}