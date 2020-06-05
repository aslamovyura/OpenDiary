using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.CQRS.Commands.Delete;
using Application.CQRS.Queries.Get;
using Application.DTO;
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
    public class PostsControllerTests : ControllerTestsFixture
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
    }
}