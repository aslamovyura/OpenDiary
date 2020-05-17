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
    /// Update topic.
    /// </summary>
    public class UpdateTopicCommand : IRequest
    {
        /// <summary>
        /// Topic DTO.
        /// </summary>
        public TopicDTO Model { get; set; }

        /// <summary>
        /// Update topic.
        /// </summary>
        public class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand>
        {
            private readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            public UpdateTopicCommandHandler(IApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            /// <summary>
            /// Update topic.
            /// </summary>
            /// <param name="request">Request command.</param>
            /// <param name="cancellationToken"></param>
            /// <returns>Void value.</returns>
            public async Task<Unit> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var post = _context.Topics.Where(p => p.Id == request.Model.Id).SingleOrDefault();

                if (post == null)
                {
                    throw new NotFoundException(nameof(Topic));
                }

                post.Text = request.Model.Text;
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}