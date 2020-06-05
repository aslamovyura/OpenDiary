using System;
using System.Collections.Generic;
using System.Security.Principal;
using Application.DTO;
using Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUI.ViewModels;
using WebUI.ViewModels.Posts;
using WebUI.ViewModels.Profile;

namespace UnitTests.Controllers
{
    public class ControllerTestsFixture
    {
        
        /// <summary>
        /// Generate single post.
        /// </summary>
        /// <returns>Single post.</returns>
        public PostDTO GetPost()
        {
            var post = new PostDTO
            {
                Id = 1,
                Author = "Author_One",
                AuthorId = 1,
                Date = DateTime.Parse("01/01/2020"),
                Text = "Text_One",
                Title = "Title_One",
                Topic = "Topic_One",
                TopicId = 1,
            };
            return post;
        }

        /// <summary>
        /// Generate single post view model.
        /// </summary>
        /// <returns>Single post view model.</returns>
        public PostViewModel GetPostViewModel()
        {
            var post = new PostViewModel
            {
                Id = 1,
                Author = "Author_One",
                AuthorId = 1,
                Date = DateTime.Parse("01/01/2020"),
                Text = "Text_One",
                Title = "Title_One",
                Topic = "Topic_One",
                TopicId = 1,
                AuthorAvatar = null,
                Comments = GetCommentViewModels(),
                CurrentReaderId = 1,

            };
            return post;
        }

        /// <summary>
        /// Generate single edit post view model.
        /// </summary>
        /// <returns>Singel view model.</returns>
        public EditPostViewModel GetEditPostViewModel()
        {
            var post = new EditPostViewModel()
            {
                Id = 1,
                Text = "Text_One",
                Title = "Title_One",
                Topic = "Topic_One",
                TopicId = 1,
            };
            return post;
        }

        /// <summary>
        /// Generate posts collection. 
        /// </summary>
        /// <returns>Posts collection.</returns>
        public ICollection<PostDTO> GetPosts()
        {
            var posts = new List<PostDTO>()
            {
                new PostDTO
                {
                    Id = 1,
                    Author = "Author_One",
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    Text = "Text_One",
                    Title = "Title_One",
                    Topic = "Topic_One",
                    TopicId = 1,
                },

                new PostDTO
                {
                    Id = 2,
                    Author = "Author_One",
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    Text = "Text_Two",
                    Title = "Title_Two",
                    Topic = "Topic_Two",
                    TopicId = 1,
                }
            };
            return posts;
        }

        /// <summary>
        /// Generate collection of post view models.
        /// </summary>
        /// <returns>Collection of post view models.</returns>
        public ICollection<PostViewModel> GetPostViewModels()
        {
            var posts = new List<PostViewModel>()
            {
                new PostViewModel
                {
                    Id = 1,
                    Author = "Author_One",
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    Text = "Text_One",
                    Title = "Title_One",
                    Topic = "Topic_One",
                    TopicId = 1,
                    AuthorAvatar = null,
                    Comments = null,
                    CurrentReaderId = 1,
                },

                new PostViewModel
                {
                    Id = 2,
                    Author = "Author_One",
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    Text = "Text_Two",
                    Title = "Title_Two",
                    Topic = "Topic_Two",
                    TopicId = 1,
                    AuthorAvatar = null,
                    Comments = null,
                    CurrentReaderId = 1,
                }
            };
            return posts;
        }

        /// <summary>
        /// Generate author DTO. 
        /// </summary>
        /// <returns>Author DTO.</returns>
        public AuthorDTO GetAuthor()
        {
            return new AuthorDTO
            {
                Id = 1,
                UserId = "someId",
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDate = DateTime.Parse("01/01/2020"),
                Email = "some@email.com",
            };
        }

        /// <summary>
        /// Generate collection of author DTO. 
        /// </summary>
        /// <returns>Collection of author DTO.</returns>
        public IEnumerable<AuthorDTO> GetAuthors()
        {
            var authors = new List<AuthorDTO>()
            {
                new AuthorDTO
                {
                    Id = 1,
                    UserId = "someId",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    BirthDate = DateTime.Parse("01/01/2020"),
                    Email = "some@email.com",
                },
                new AuthorDTO
                {
                    Id = 2,
                    UserId = "someId_Two",
                    FirstName = "FirstName_Two",
                    LastName = "LastName_Two",
                    BirthDate = DateTime.Parse("01/01/2020"),
                    Email = "some@email.com",
                },
            };
            return authors;
        }

        /// <summary>
        /// Generate author view model. 
        /// </summary>
        /// <returns>Author view model.</returns>
        public AuthorViewModel GetAuthorVieModel()
        {
            var author = new AuthorViewModel
            {
                AuthorId = 1,
                UserId = "someId",
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDate = DateTime.Parse("01/01/2020"),
                Email = "some@email.com",
                Avatar = null,
                PostsNumber = 2,
                CommentsNumber = 2,
            };
            return author;
        }


        /// <summary>
        /// Generate collection of author view models. 
        /// </summary>
        /// <returns>Collection of author view models.</returns>
        public ICollection<AuthorViewModel> GetAuthorVieModels()
        {
            var authors = new List<AuthorViewModel>()
            {
                new AuthorViewModel
                {
                    AuthorId = 1,
                    UserId = "someId",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    BirthDate = DateTime.Parse("01/01/2020"),
                    Email = "some@email.com",
                    Avatar = null,
                    PostsNumber = 2,
                    CommentsNumber = 2,

                },
                new AuthorViewModel
                {
                    AuthorId = 2,
                    UserId = "someId_Two",
                    FirstName = "FirstName_Two",
                    LastName = "LastName_Two",
                    BirthDate = DateTime.Parse("01/01/2020"),
                    Email = "some@email.com",
                    Avatar = null,
                    PostsNumber = 0,
                    CommentsNumber = 0,
                },
            };
            return authors;
        }

        /// <summary>
        /// Generate topic DTO.
        /// </summary>
        /// <returns>topic DTO.</returns>
        public TopicDTO GetTopic()
        {
            return new TopicDTO
            {
                Id = 1,
                Text = "Topic"
            };
        }

        /// <summary>
        /// Generate collection of topic dto.
        /// </summary>
        /// <returns>Collection of topic dto.</returns>
        public IEnumerable<TopicDTO> GetTopics()
        {
            var topics = new List<TopicDTO>()
            {
                new TopicDTO
                {
                    Id = 1,
                    Text = "Topic_One",
                },

                new TopicDTO
                {
                    Id = 2,
                    Text = "Topic_Two",
                },
            };
            return topics;
        }

        /// <summary>
        /// Generate collection of topic view models.
        /// </summary>
        /// <returns>Collection of topic view models.</returns>
        public ICollection<TopicViewModel> GetTopicViewModels()
        {
            var topics = new List<TopicViewModel>()
            {
                new TopicViewModel
                {
                    Id = 1,
                    Text = "Topic_One",
                },

                new TopicViewModel
                {
                    Id = 2,
                    Text = "Topic_Two",
                },
            };
            return topics;
        }

        /// <summary>
        /// Generate collection of comments DTO.
        /// </summary>
        /// <returns>Collection of comments DTO.</returns>
        public ICollection<CommentDTO> GetComments()
        {
            var comments = new List<CommentDTO>()
            {
                new CommentDTO
                {
                    Id = 1,
                    AuthorId = 1,
                    Date = DateTime.Parse("10/01/2020"),
                    PostId = 1,
                    Text = "Comment_One",
                },

                new CommentDTO
                {
                    Id = 2,
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    PostId = 1,
                    Text = "Comment_One",
                },
            };
            return comments;
        }

        /// <summary>
        /// Generate collection of comment view models.
        /// </summary>
        /// <returns>Collection of comment view models.</returns>
        public ICollection<CommentViewModel> GetCommentViewModels()
        {
            var comments = new List<CommentViewModel>()
            {
                new CommentViewModel
                {
                    Id = 1,
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    PostId = 1,
                    Text = "Comment_One",
                    Age = 2,
                    AgeUnits = AgeUnits.Day,
                    Author = "Author_One",
                    AuthorAvatar = null,
                },
                new CommentViewModel
                {
                    Id = 2,
                    AuthorId = 1,
                    Date = DateTime.Parse("01/01/2020"),
                    PostId = 1,
                    Text = "Comment_Two",
                    Age = 2,
                    AgeUnits = AgeUnits.Day,
                    Author = "Author_One",
                    AuthorAvatar = null,
                },
            }; 
            return comments;
        }

        /// <summary>
        /// Generate profile view model. 
        /// </summary>
        /// <returns>Profile view model.</returns>
        public ProfileViewModel GetProfileViewModel()
        {
            var profile = new ProfileViewModel
            {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    BirthDate = "01/01/2020",
                    Email = "some@email.com",
                    Age = 1,
                    About = "About",
                    Hobbies = "Hobbies",
                    Profession = "Profession",
                    CurrentReaderId = 1,
                    Avatar = null,
                    TotalPostsNumber = 2,
                    TotalCommentsNumber = 2,

            };
            return profile;
        }

        /// <summary>
        /// Generate fake controller context.
        /// </summary>
        /// <returns>Fake controller context.</returns>
        public ControllerContext GetFakeContext()
        {
            var fakeContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeContext.Setup(x => x.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
            return context;
        }
    }
}