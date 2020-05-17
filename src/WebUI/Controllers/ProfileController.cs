using System;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels.Profile;

namespace WebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public ProfileController(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string userName)
        {
            if (userName == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get id of application user.
            var userId = await _identityService.GetUserIdByNameAsync(userName);

            // Get author current author.
            var authorQuery = new GetAuthorByUserIdQuery { UserId = userId };
            var authorDTO = await _mediator.Send(authorQuery);
            authorDTO.Email = await _identityService.GetEmailByIdAsync(authorDTO.UserId);

            // Calculate author age.
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Now - authorDTO.BirthDate;
            int ageYears = (zeroTime + span).Year - 1;

            // Calculate author statistics.
            var postsQuery = new GetPostsByAuthorIdQuery { AuthorId = authorDTO.Id };
            var postsDTO = await _mediator.Send(postsQuery);
            var postsNumber = postsDTO.Count;

            // TODO : add comments query.
            var commentsNumber = 0;

            // Create view model.
            var model = new ProfileViewModel
            {
                FirstName = authorDTO.FirstName,
                LastName = authorDTO.LastName,
                Email = authorDTO.Email,
                BirthDate = authorDTO.BirthDate.ToString("MMMM d, yyyy"),
                Age = ageYears,
                TotalPostsNumber = postsNumber,
                TotalCommentsNumber = commentsNumber,
                Posts = postsDTO,
            };

            return View(model);


            //var user = await _userManager.FindByNameAsync(userName);

            //if (user != null)
            //{
            //    var author = await _db.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id);

            //    // Calculate user age.
            //    DateTime zeroTime = new DateTime(1, 1, 1);
            //    TimeSpan span = DateTime.Now - author.BirthDate;
            //    int ageYears = (zeroTime + span).Year - 1;

            //    // Calculate user statistics.
            //    var posts = await _db.Posts.Where(post => post.AuthorId == author.Id)
            //        .OrderByDescending(post => post.Date)
            //        .ToListAsync();

            //    var postsNumber = posts.Count;
            //    var commentsNumber = _db.Comments.Where(post => post.AuthorId == author.Id)
            //        .OrderByDescending(post => post.Date)
            //        .ToListAsync().GetAwaiter().GetResult().Count;

            //    ViewUserViewModel model = new ViewUserViewModel
            //    {
            //        FirstName = author.FirstName,
            //        LastName = author.LastName,
            //        Email = user.Email,
            //        BirthDate = author.BirthDate.ToString("MMMM d, yyyy"),
            //        Age = ageYears,
            //        TotalPostsNumber = postsNumber,
            //        TotalCommentsNumber = commentsNumber,
            //        Posts = posts
            //    };
            //    return View(model);
            //}

            //// Default action
            //return RedirectToAction("Index", "Home");
        }
    }
}