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
    /// Defice class to get post info.
    /// </summary>
    public class GetAuthorQuery : IRequest<AuthorDTO>
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Handler of the author queries.
        /// </summary>
        public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Model mapper.</param>
            public GetAuthorQueryHandler(IApplicationDbContext context, IMapper mapper)
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
            public async Task<AuthorDTO> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await _context.Authors.Where(a => a.Id == request.Id).SingleOrDefaultAsync();
                var author = _mapper.Map<AuthorDTO>(entity);

                return author;
            }
        }
    }
}