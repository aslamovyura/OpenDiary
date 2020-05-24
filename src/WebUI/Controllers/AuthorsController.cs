using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Collections.Generic;
using Application.DTO;
using Application.CQRS.Queries.Get;
using MediatR;
using WebUI.ViewModels;
using AutoMapper;

namespace CustomIdentityApp.Controllers
{
    /// <summary>
    /// Controller to manage authors.
    /// </summary>
    public class AuthorsController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of user controller.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthorsController( IIdentityService identityService,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Show page with user/aurhors list.
        /// </summary>
        /// <returns>Page with list of users.</returns>
        public async Task<IActionResult> Index()
        {
            var authorsQuery = new GetAuthorsQuery();
            var authors = await _mediator.Send(authorsQuery);

            var models = _mapper.Map<IEnumerable<AuthorDTO>, IEnumerable<AuthorViewModel>>(authors);

            foreach (var model in models)
            {
                model.Email = await _identityService.GetEmailByIdAsync(model.UserId);
            }

            return View(models);
        }
    }
}