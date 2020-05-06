using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.ViewModels.Posts;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="context"></param>
        public PostsController(SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = signInManager.UserManager;
            _db = context;
        }

        /// <summary>
        /// View the list of latest posts.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts.ToListAsync();
            foreach (var post in posts)
            {
                post.User = await _db.Users.FirstOrDefaultAsync(user => user.Id == post.UserId);
                post.Topic = await _db.Topics.FirstOrDefaultAsync(topic => topic.Id == post.TopicId);
            }

            var postsSorted = posts.OrderByDescending(post => post.Date);

            return View(postsSorted);
        }

        /// <summary>
        /// Read full post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Read(int postId)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(post => post.Id == postId);
            if (post == null)
            {
                return NotFound();
            }
            
            post.User = await _db.Users.FirstOrDefaultAsync(user => user.Id == post.UserId);
            post.Topic = await _db.Topics.FirstOrDefaultAsync(topic => topic.Id == post.TopicId);
            return View(post);
        }

        /// <summary>
        /// Show all posts.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllPosts()
        {
            var posts = await _db.Posts.ToListAsync();
            foreach (var post in posts)
            {
                post.User = await _db.Users.FirstOrDefaultAsync(user => user.Id == post.UserId);
                post.Topic = await _db.Topics.FirstOrDefaultAsync(topic => topic.Id == post.TopicId);
            }

            posts.OrderByDescending(post => post.Date);

            return View(posts);
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
            if(ModelState.IsValid)
            {
                Topic topic = new Topic
                {
                    Text = model.Topic
                };
                _db.Topics.Add(topic); 

                Post post = new Post
                {
                    Title = model.Title,
                    Text = model.Text,
                    Topic = topic,
                    User = await GetUser(),
                    Date = DateTime.Now
                };

                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
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
            var post = await _db.Posts.FirstOrDefaultAsync(post => post.Id == postId);
            if (post == null)
            {
                return NotFound();
            }

            var topic = await _db.Topics.FirstOrDefaultAsync(topic => topic.Id == post.TopicId);

            EditPostViewModel model = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                Topic = topic.Text,
                Date = post.Date
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
                var post = await _db.Posts.FirstOrDefaultAsync(post => post.Id == model.Id);
                if (post != null)
                {
                    // Update topic
                    var topic = await _db.Topics.FirstOrDefaultAsync(topic => topic.Id == post.TopicId);
                    if (topic != null)
                    {
                        topic.Text = model.Text;
                        _db.Topics.Update(topic);
                    }

                    // Update post
                    post.Title = model.Title;
                    post.Text = model.Text;
                    post.Date = model.Date;
                    _db.Posts.Update(post);

                    // Save changes
                    await _db.SaveChangesAsync();

                    // TODO : Redirect to admin list
                    return RedirectToAction("Index");
                }
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
            var post = await _db.Posts.FirstOrDefaultAsync(post => post.Id == postId);
            if (post != null)
            {
                _db.Posts.Remove(post);
               await _db.SaveChangesAsync();
            }
            return RedirectToAction("AllPosts", "Posts");
        }

            private async Task<User> GetUser()
        {
            return await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
    }
}