using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Update
{
    /// <summary>
    /// Update author.
    /// </summary>
    public class UpdateAuthorCommand : IRequest
    {
        /// <summary>
        /// Author data transfer object.
        /// </summary>
        public AuthorDTO Model { get; set; }

        /// <summary>
        /// Update author.
        /// </summary>
        public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public UpdateAuthorCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Update author.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns>Void value.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public async Task<Unit> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var author = _context.Authors.Where(a => a.Id == request.Model.Id).SingleOrDefault();
                if (author == null)
                {
                    throw new NotFoundException(nameof(Author), author.Id);  // TODO: add custorm exception.
                }

                author.FirstName = request.Model.FirstName;
                author.LastName = request.Model.LastName;
                author.BirthDate = request.Model.BirthDate;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}