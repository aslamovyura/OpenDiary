using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Create
{
    /// <summary>
    /// Create new topic.
    /// </summary>
    public class CreateTopicCommand : IRequest<int>
    {
        /// <summary>
        /// Topic DTO.
        /// </summary>
        public TopicDTO Model { get; set; }

        public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            /// <summary>
            /// Constructor with parameters.
            /// </summary>
            /// <param name="context">Application context.</param>
            /// <param name="mapper">Mapper.</param>
            /// <exception cref="ArgumentNullException"></exception>
            public CreateTopicCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            /// <summary>
            /// Create new topic.
            /// </summary>
            /// <param name="request">Request.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>New post Id.</returns>
            public async Task<int> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
            {
                request = request ?? throw new ArgumentNullException(nameof(request));

                var entity = await _context.Topics.FirstOrDefaultAsync(t => t.Text == request.Model.Text);

                if (entity == null)
                {
                    entity = _mapper.Map<Topic>(request.Model);
                    _context.Topics.Add(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return entity.Id;
            }
        }
    }
}