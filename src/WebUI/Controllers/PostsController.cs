using System;
using System.Linq;
using System.Threading.Tasks;
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
using System.Collections.Generic;
using Infrastructure.Extentions;
using WebUI.ViewModels;
using WebUI.ViewModels.Posts;

namespace WebUI.Controllers
{
    /// <summary>
    /// Controller to manage posts.
    /// </summary>
    [Authorize]
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
        public PostsController( IMediator mediator,
                                IIdentityService identityService,
                                IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Show the list of posts.
        /// </summary>
        /// <param name="authorId">Author identifier.</param>
        /// <returns>View with posts.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index(int authorId = default)
        {
            IRequest<ICollection<PostDTO>> postsQuery;
            if (authorId == default)
                postsQuery = new GetPostsQuery();
            else
                postsQuery = new GetPostsByAuthorIdQuery { AuthorId = authorId };

            // Get all posts.
            var postsDTO = await _mediator.Send(postsQuery);
            var posts = postsDTO.OrderByDescending(post => post.Date);

            var models = _mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(posts);

            foreach (var model in models)
            {
                var authorQuery = new GetAuthorQuery { Id = model.AuthorId };
                var author = await _mediator.Send(authorQuery);

                var topicQuery = new GetTopicQuery { Id = model.TopicId };
                var topic = await _mediator.Send(topicQuery);

                model.AuthorId = author.Id;
                model.Author = author.FirstName + " " + author.LastName;
                model.Topic = topic.Text;
            }

            return View(models);
        }

        /// <summary>
        /// Read full post.
        /// </summary>
        /// <param name="postId">Post identifier.</param>
        /// <returns>Page to read the full post.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Read(int postId)
        {
            // TODO : add check for empty post ID.
            if (postId == default)
            {
                return NotFound();
            }

            // Get post.
            var postQuery = new GetPostQuery { Id = postId };
            var post = await _mediator.Send(postQuery);

            // Add author information.
            var authorQuery = new GetAuthorQuery { Id = post.AuthorId };
            var author = await _mediator.Send(authorQuery);
            post.Author = author.FirstName + " " + author.LastName;

            // Add topic information.
            var topicQuery = new GetTopicQuery { Id = post.TopicId };
            var topic = await _mediator.Send(topicQuery);
            post.Topic = topic.Text;

            // Get comments for current post.
            var commentsQuery = new GetCommentsByPostIdQuery { PostId = post.Id };
            var comments = await _mediator.Send(commentsQuery);

            // Create post view model.
            var model = _mapper.Map<PostDTO, PostViewModel>(post);

            // Check current user if he/she is an author of the current post.
            var userName = HttpContext.User.Identity.Name;
            if (userName == null)
            {
                model.CurrentReaderId = default;
            }
            else
            {
                var userId = await _identityService.GetUserIdByNameAsync(userName);
                var reader = await _mediator.Send(new GetAuthorByUserIdQuery { UserId = userId });

                if (reader == null)
                    model.CurrentReaderId = default;
                else
                    model.CurrentReaderId = reader.Id;
            }
            
            // Check post comments
            model.Comments = _mapper.Map<ICollection<CommentDTO>, ICollection<CommentViewModel>>(comments);

            foreach(var comment in model.Comments)
            {
                var commentAuthorQuery = new GetAuthorQuery { Id = comment.AuthorId };
                var commentAuthor = await _mediator.Send(commentAuthorQuery);

                comment.Author = commentAuthor.FirstName + " " + commentAuthor.LastName;

                (var age, var units) = comment.Date.Age();
                comment.Age = age;
                comment.AgeUnits = units;
            }

            return View(model);
        }

        /// <summary>
        /// Show all posts.
        /// </summary>
        /// <returns>Return list of posts.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllPosts()
        {
            // Get all posts.
            var postsQuery = new GetPostsQuery();
            var postsDTO = await _mediator.Send(postsQuery);
            var posts = postsDTO.OrderByDescending(post => post.Date);

            var models = _mapper.Map<IEnumerable<PostDTO>, IEnumerable <PostViewModel>>(posts);

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
        /// <returns>View to create new post.</returns>
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var topicsQuery = new GetTopicsQuery();
            var topicsDTO = await _mediator.Send(topicsQuery);

            var topics = _mapper.Map<IEnumerable<TopicDTO>, ICollection<TopicViewModel>>(topicsDTO);

            var model = new CreatePostViewModel { Topics = topics };

            return View(model);
        }

        /// <summary>
        /// Create new post.
        /// </summary>
        /// <param name="model">View model of the post.</param>
        /// <returns>Create new post and redirect to the page with posts.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel model)
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
                
                return RedirectToAction("Index", "Posts");
            }

            return View(model);
        }

        /// <summary>
        /// Show page to edit user's info.
        /// </summary>
        /// <param name="postId">Post identifier.</param>
        /// <returns>View with EditPostViewModel.</returns>
        [Authorize]
        public async Task<IActionResult> Edit(int postId)
        {
            // Get post.
            var postQuery = new GetPostQuery { Id = postId };
            var postDTO = await _mediator.Send(postQuery);

            // Get topic.
            var topicId = postDTO.TopicId;
            var topicQuery = new GetTopicQuery { Id = topicId };
            var topicDTO = await _mediator.Send(topicQuery);

            var model = _mapper.Map<PostDTO, EditPostViewModel>(postDTO);
            model.Topic = topicDTO.Text;

            // Get topics list.
            var topicsDTO = await _mediator.Send(new GetTopicsQuery());
            var topics = _mapper.Map<IEnumerable<TopicDTO>, ICollection<TopicViewModel>>(topicsDTO);
            model.Topics = topics;

            return View(model);
        }

        /// <summary>
        /// Process input date to edit user post.
        /// </summary>
        /// <param name="model">View model to edit post.</param>
        /// <returns>List of posts.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                /* Update topic of the post.
                We use `create` command instead of `update`,
                because one topic can relate to multiple posts (one-to-many relations)*/
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

                // Update post.
                var postDTO = _mapper.Map<EditPostViewModel, PostDTO>(model);
                postDTO.TopicId = topicId;
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

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Delete Post.
        /// </summary>
        /// <param name="postId">Post identifier</param>
        /// <returns>Delete current post and redirect to page with posts.</returns>
        public async Task<IActionResult> Delete(int postId, string returnUrl=default)
        {
            var postCommand = new DeletePostCommand { Id = postId };
            await _mediator.Send(postCommand);


            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Posts");
            else
                return RedirectToAction(returnUrl);
        }
    }
}