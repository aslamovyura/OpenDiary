using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Create new author.
    /// </summary>
    public class CreateAuthorCommand : IRequest<int>
    {
        /// <summary>
        /// Author DTO.
        /// </summary>
        public AuthorDTO Model { get; set; }

        public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Mapper.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public CreateAuthorCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Create new author.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>New post Id.</returns>
            public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = _mapper.Map<Author>(request.Model);

                _context.Authors.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}