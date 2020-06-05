using System;
using System.Collections.Generic;
using Application.DTO;
using Application.Enums;
using WebUI.ViewModels;
using WebUI.ViewModels.Posts;

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
        /// <returns>author DTO.</returns>
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
                    Age = 1,
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
                    Text = "Comment_One",
                    Age = 2,
                    AgeUnits = AgeUnits.Day,
                    Author = "Author_One",
                    AuthorAvatar = null,
                },
            };
            return comments;
        }
    }
}