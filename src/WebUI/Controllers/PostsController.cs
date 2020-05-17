using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Application.CQRS.Commands.Create;
using MediatR;
using Application.Interfaces;
using Application.CQRS.Queries.Get;
using Application.Exceptions;
using Application.CQRS.Commands.Update;
using Application.CQRS.Commands.Delete;
using AutoMapper;

namespace WebUI.Controllers
{
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="identityService"></param>
        /// <param name="mapper"></param>
        public PostsController(IMediator mediator, IIdentityService identityService, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// View the list of latest posts.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // Get all posts.
            var postsQuery = new GetPostsQuery();
            var posts = await _mediator.Send(postsQuery);
            var models = posts.OrderByDescending(post => post.Date);

            //var models = _mapper.Map<IEnumerable<PostViewModel>>(postsSorted);

            foreach (var model in models)
            {
                var authorQuery = new GetAuthorQuery { Id = model.AuthorId };
                var author = await _mediator.Send(authorQuery);

                var topicQuery = new GetTopicQuery { Id = model.TopicId };
                var topic = await _mediator.Send(topicQuery);

                model.Author = author.FirstName + " " + author.LastName;
                model.Topic = topic.Text;
            }

            return View(models);
        }

        /// <summary>
        /// Read full post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Read(int postId)
        {
            // Get post.
            var postQuery = new GetPostQuery { Id = postId };
            var post = await _mediator.Send(postQuery);

            //var model = _mapper.Map<PostViewModel>(post);

            // Add author information.
            var authorQuery = new GetAuthorQuery { Id = post.AuthorId };
            var author = await _mediator.Send(authorQuery);
            post.Author = author.FirstName + " " + author.LastName;

            // Add topic information.
            var topicQuery = new GetTopicQuery { Id = post.TopicId };
            var topic = await _mediator.Send(topicQuery);
            post.Topic = topic.Text;

            return View(post);
        }

        /// <summary>
        /// Show all posts.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllPosts()
        {
            // Get all posts.
            var postsQuery = new GetPostsQuery();
            var postsDTO = await _mediator.Send(postsQuery);
            var models = postsDTO.OrderByDescending(post => post.Date);

            //var models = _mapper.Map<IEnumerable<PostViewModel>>(posts);

            foreach (var model in models)
            {
                var authorQuery = new GetAuthorQuery { Id = model.AuthorId };
                var author = await _mediator.Send(authorQuery);

                var topicQuery = new GetTopicQuery { Id = model.TopicId };
                var topic = await _mediator.Send(topicQuery);

                model.Author = author.FirstName + " " + author.LastName;
                model.Topic = topic.Text;
            }

            return View(models);
        }

        /// <summary>
        /// Show page to create new post.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create() => View();

        /// <summary>
        /// Create new post.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add new topic.
                var topicDTO = new TopicDTO { Text = model.Topic };
                var topicCommand = new CreateTopicCommand { Model = topicDTO };

                int topicId;

                try
                {
                    topicId = await _mediator.Send(topicCommand);
                }
                catch (RequestValidationException failures)
                {
                    foreach (var error in failures.Failures)
                    {
                        ModelState.AddModelError(string.Empty, error.Value[0]);
                    }
                    return View(model);
                }

                // Get current author.
                var userId = await _identityService.GetUserIdByNameAsync(HttpContext.User.Identity.Name);
                var authorQuery = new GetAuthorByUserIdQuery { UserId = userId };
                var author = await _mediator.Send(authorQuery);

                // Create new post.
                var postDTO = new PostDTO
                {
                    Title = model.Title,
                    Text = model.Text,
                    AuthorId = author.Id,
                    TopicId = topicId,
                    Date = DateTime.Now,
                };

                var postCommand = new CreatePostCommand { Model = postDTO };

                try
                {
                    await _mediator.Send(postCommand);
                }
                catch (RequestValidationException failures)
                {
                    foreach (var error in failures.Failures)
                    {
                        ModelState.AddModelError(string.Empty, error.Value[0]);
                    }
                    return View(model);
                }
                
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        /// <summary>
        /// Show page to edit user's info.
        /// </summary>
        /// <param name="postId">Post identifier.</param>
        /// <returns>View with EditPostViewModel.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int postId)
        {
            // Get post.
            var postQuery = new GetPostQuery { Id = postId };
            var postDTO = await _mediator.Send(postQuery);

            // Get topic.
            var topicId = postDTO.TopicId;
            var topicQuery = new GetTopicQuery { Id = topicId };
            var topicDTO = await _mediator.Send(topicQuery);

            // Create view model.
            var model = new EditPostViewModel
            {
                Id = postDTO.Id,
                TopicId = postDTO.TopicId,
                Title = postDTO.Title,
                Text = postDTO.Text,
                Topic = topicDTO.Text,
                Date = postDTO.Date
            };
            return View(model);
        }

        /// <summary>
        /// Process input date to edit user post.
        /// </summary>
        /// <param name="model">View model to edit post.</param>
        /// <returns>List of posts.</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update post.
                var postDTO = new PostDTO
                {
                    Id = model.Id,
                    Title = model.Title,
                    Text = model.Text,
                    Date = model.Date, 
                };

                var postCommand = new UpdatePostCommand { Model = postDTO };

                try
                {
                    await _mediator.Send(postCommand);
                }
                catch (RequestValidationException failures)
                {
                    foreach (var error in failures.Failures)
                    {
                        ModelState.AddModelError(string.Empty, error.Value[0]);
                    }
                    return View(model);
                }

                // Update topic of the post.
                var topicDTO = new TopicDTO
                {
                    Id = model.TopicId,
                    Text = model.Topic,
                };

                // TODO : Use "CreateTopicCommand" instead or modify "UpdateTopicCommand" (to avoid dublicates).
                var topicCommand = new UpdateTopicCommand { Model = topicDTO };

                try
                {
                    await _mediator.Send(topicCommand);
                }
                catch (RequestValidationException failures)
                {
                    foreach (var error in failures.Failures)
                    {
                        ModelState.AddModelError(string.Empty, error.Value[0]);
                    }
                    return View(model);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Delete Post.
        /// </summary>
        /// <param name="postId">Post identifier</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int postId)
        {
            var postCommand = new DeletePostCommand { Id = postId };
            await _mediator.Send(postCommand);

            return RedirectToAction("AllPosts", "Posts");
        }
    }
}