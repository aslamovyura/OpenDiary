using System;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.ViewComponents
{
    /// <summary>
    /// View component for user login panel.
    /// </summary>
    public class LoginViewComponent : ViewComponent
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Define view component for user login panel.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="identityService">Identity service.</param>
        /// <param name="mapper">Models automapper.</param>
        public LoginViewComponent(IIdentityService identityService,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Invoke login panel.
        /// </summary>
        /// <returns>Login view component.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                return Content(string.Empty);
            }

            var userId = await _identityService.GetUserIdByNameAsync(userName);

            var authorQuery = new GetAuthorByUserIdQuery { UserId = userId };
            var authorDTO = await _mediator.Send(authorQuery);

            var model = _mapper.Map<AuthorDTO, AuthorViewModel>(authorDTO);

            return View("Login", model);
        }
    }
}