using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Commands.Delete
{
    public class DeleteTopicCommand : IRequest
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Delete topic request.
        /// </summary>
        public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor without parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            public DeleteTopicCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Delete topic from DB.
            /// </summary>
            /// <param name="request">Delete command.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Value.</returns>
            public async Task<Unit> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var topic = _context.Topics.Where(t => t.Id == request.Id).SingleOrDefault();

                if(topic == null)
                {
                    throw new NotFoundException(nameof(Topic), request.Id);
                }

                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}