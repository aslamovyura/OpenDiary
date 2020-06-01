using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.CQRS.Commands.Delete;
using Application.DTO;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;


namespace WebUI.Controllers
{
    /// <summary>
    /// Controller to manage user comments.
    /// </summary>
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public CommentsController(IMediator mediator,
                                   IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Add new comment to the current post.
        /// </summary>
        /// <param name="model">View model of comment.</param>
        /// <returns>Page with post.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(CommentViewModel model)
        {
            var commentDTO = _mapper.Map<CommentViewModel, CommentDTO>(model);
            commentDTO.Date = DateTime.Now;

            var commentCommand = new CreateCommentCommand { Model = commentDTO };

            try
            {
                await _mediator.Send(commentCommand);
            }
            catch (RequestValidationException failures)
            {
                foreach (var error in failures.Failures)
                {
                    ModelState.AddModelError(string.Empty, error.Value[0]);
                }
            }

            return RedirectToAction("Read", "Posts", new { id = commentDTO.PostId });
        }

        /// <summary>
        /// Delete comment to the current post.
        /// </summary>
        /// <param name="id">Comment identifier.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Page with post.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id = default, string returnUrl = default)
        {
            if (id != default)
            {
                var commentCommand = new DeleteCommentCommand { Id = id };
                await _mediator.Send(commentCommand);
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Posts");
            else
                return Redirect(returnUrl);
        }
    }
}