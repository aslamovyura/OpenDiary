using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.CQRS.Commands.Delete;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Application.Enums;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUI.Controllers;
using WebUI.ViewModels;
using WebUI.ViewModels.Posts;
using Xunit;

namespace UnitTests.Controllers
{
    public class PostsControllerTests
    {
        [Fact]
        public void Index_WhenIdIsNull_Return_ViewResultWithAllPosts()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPosts()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopic()));


            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(It.IsAny<IEnumerable<PostDTO>>()))
                .Returns(GetPostViewModels());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            // Act
            var result = controller.Index().GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var models = Assert.IsAssignableFrom<IEnumerable<PostViewModel>>(viewResult.ViewData.Model);

            var modelsCount = 0;
            foreach (var model in models)
                modelsCount++;

            Assert.Equal(2, modelsCount);
        }

        [Fact]
        public void Index_WhenIdIsNotNull_Return_ViewResultWithPostsOfSingleAuthor()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPosts()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopic()));


            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(It.IsAny<IEnumerable<PostDTO>>()))
                .Returns(GetPostViewModels());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            // Act
            var result = controller.Index(id: 1).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var models = Assert.IsAssignableFrom<IEnumerable<PostViewModel>>(viewResult.ViewData.Model);

            var modelsCount = 0;
            foreach (var model in models)
                modelsCount++;

            Assert.Equal(2, modelsCount);
        }

        [Fact]
        public void Read_WhenIdIsNull_Return_NotFoundResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();
            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            // Act
            var result = controller.Read(default).GetAwaiter().GetResult();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Read_WhenIdIsNotNull_Return_ViewResultWithPost()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPost()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopic()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetCommentsByPostIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetComments()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

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

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(identity => identity.GetUserIdByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("someId"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<PostDTO, PostViewModel>(It.IsAny<PostDTO>()))
                .Returns(GetPostViewModel());

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(It.IsAny<IEnumerable<CommentDTO>>()))
                .Returns(GetCommentViewModels());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            controller.ControllerContext = context;

            // Act
            var result = controller.Read(id: 1).GetAwaiter().GetResult();

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            var models = Assert.IsAssignableFrom<PostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Create_Return_ViewResultToCreatePost()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopics()));

            var identityMock = new Mock<IIdentityService>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<TopicDTO>, IEnumerable<TopicViewModel>>(It.IsAny<IEnumerable<TopicDTO>>()))
                .Returns(GetTopicViewModels());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            // Act
            var result = controller.Create().GetAwaiter().GetResult();

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            var models = Assert.IsAssignableFrom<CreatePostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void CreatePost_WithInvalidModel_Return_CreateViewAgain()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            controller.ModelState.AddModelError("Title", "TitleRequired");
            var model = new CreatePostViewModel { };

            // Act
            var result = controller.Create(model).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreatePostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void CreatePost_WithValidModel_Return_RedirectToReadAction()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

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

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);
            controller.ControllerContext = context;

            var model = new CreatePostViewModel
            {
                Text = "PostPostPostPostPostPostPostPostPostPostPostPost",
                Title = "Title",
                Topic = "Topic",
            };

            // Act
            var result = controller.Create(model).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Read", redirectToActionResult.ActionName);
            Assert.Equal("Posts", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Edit_WithInvaliId_Return_NotFoundResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            PostDTO post = null;
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(post));

            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);
            var testId = 0;

            // Act
            var result = controller.Edit(testId).GetAwaiter().GetResult();

            // Arrange
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(testId, notFoundObjectResult.Value);
        }

        [Fact]
        public void Edit_WithValiId_Return_ViewResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPost()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopic()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTopics()));

            var identityMock = new Mock<IIdentityService>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<PostDTO, EditPostViewModel>(It.IsAny<PostDTO>()))
                .Returns(GetEditPostViewModel());

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<TopicDTO>, ICollection<TopicViewModel>>(It.IsAny<IEnumerable<TopicDTO>>()))
                .Returns(GetTopicViewModels());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);
            var testId = 1;

            // Act
            var result = controller.Edit(testId).GetAwaiter().GetResult();

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditPostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void EditPost_WithInvalidModel_Return_EditViewAgain()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            controller.ModelState.AddModelError("Title", "TitleRequired");
            var model = new EditPostViewModel { };

            // Act
            var result = controller.Edit(model).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditPostViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void EditPost_WithValidModel_Return_RedirectToReadAction()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<EditPostViewModel, PostDTO>(It.IsAny<EditPostViewModel>()))
                .Returns(GetPost());

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);

            var postId = 1;
            var model = new EditPostViewModel
            {
                Id = postId,
                TopicId = 1,
                Text = "PostPostPostPostPostPostPostPostPostPostPostPost",
                Title = "Title",
                Topic = "Topic",
            };

            // Act
            var result = controller.Edit(model).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Read", redirectToActionResult.ActionName);
            Assert.Equal("Posts", redirectToActionResult.ControllerName);
        }

        // Get single post.
        private PostDTO GetPost()
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

        // Get single post.
        private PostViewModel GetPostViewModel()
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

        [Fact]
        public void Delete_WithInvalidReturnUrl_Return_RedirectToPostsAction()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new PostsController(mediatorMock.Object,
                                                 identityMock.Object,
                                                 mapperMock.Object);
            var postId = 1;

            // Act
            var result = controller.Delete(postId).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Posts", redirectToActionResult.ControllerName);
        }

        // Get single post.
        private EditPostViewModel GetEditPostViewModel()
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

        // Generate posts 
        private ICollection<PostDTO> GetPosts()
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

        // Generate post view models
        private ICollection<PostViewModel> GetPostViewModels()
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

        // Generate author dto 
        private AuthorDTO GetAuthor()
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

        // Generate topic dto 
        private TopicDTO GetTopic()
        {
            return new TopicDTO
            {
                Id = 1,
                Text = "Topic"
            };
        }

        // Generate list of topic dto 
        private IEnumerable<TopicDTO> GetTopics()
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

        // Generate list of topic view models 
        private ICollection<TopicViewModel> GetTopicViewModels()
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

        // Generate comments dto.
        private ICollection<CommentDTO> GetComments()
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

        // Generate comments dto.
        private ICollection<CommentViewModel> GetCommentViewModels()
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