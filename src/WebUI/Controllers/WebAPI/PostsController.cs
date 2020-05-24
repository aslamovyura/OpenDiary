using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Models;

namespace WebUI.Controllers.WebAPI
{
    /// <summary>
    /// API controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Create controller to manage post via API.
        /// </summary>
        /// <param name="logger">Logger service.</param>
        /// <param name="mediator">Mediator service.</param>
        /// <param name="memoryCache">Cache.</param>
        public PostsController(ILogger<PostsController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get the whole posts collection.
        /// </summary>
        /// <returns>Posts collection.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var posts = await _mediator.Send(new GetPostsQuery());

            if (!posts.Any())
            {
                return NoContent();
            }

            var postModels = new List<PostModel>();

            foreach (var post in posts)
            {
                var author = await _mediator.Send(new GetAuthorQuery { Id = post.AuthorId });
                if (author != null)
                {
                    post.Author = author.FirstName + " " + author.LastName;
                }

                var topic = await _mediator.Send(new GetTopicQuery { Id = post.TopicId });
                if (topic != null)
                {
                    post.Topic = topic.Text;
                }

                postModels.Add(new PostModel
                {
                    Id = post.Id,
                    Date = post.Date,
                    Title = post.Title,
                    Text = post.Text,
                    AuthorId = post.AuthorId,
                    Author = post.Author,
                    TopicId = post.TopicId,
                    Topic = post.Topic
                });
            }

            _logger.LogInformation(@"Posts ({posts.Count} were successfully sent.");
            return Json(postModels);
        }

        /// <summary>
        /// Get single pos by its Id.
        /// </summary>
        /// <param name="id">Post identifier.</param>
        /// <returns>Single post.</returns>
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetPostAsync(int id)
        {
            var postQuery = new GetPostQuery {Id = id };
            var post = await _mediator.Send(postQuery);

            if (post == null)
            {
                return NoContent();
            }

            var author = await _mediator.Send(new GetAuthorQuery { Id = post.AuthorId });
            if (author != null)
            {
                post.Author = author.FirstName + " " + author.LastName;
            }

            var topic = await _mediator.Send(new GetTopicQuery { Id = post.TopicId });
            if (topic != null)
            {
                post.Topic = topic.Text;
            }

            var postModel = new PostModel
            {
                Id = post.Id,
                Date = post.Date,
                Title = post.Title,
                Text = post.Text,
                AuthorId = post.AuthorId,
                Author = post.Author,
                TopicId = post.TopicId,
                Topic = post.Topic
            };

            _logger.LogInformation(@"Post with Id={id} was successfully sent.");
            return Json(postModel);
        }
    }
}