using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Update;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUI.Controllers;
using WebUI.ViewModels.Profile;
using Xunit;

namespace UnitTests.Controllers
{
    public class ProfileControllerTests : ControllerTestsFixture
    {
        [Fact]
        public void Index_WithValidId_Returns_ViewResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPosts()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetCommentsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetComments()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));


            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(identity => identity
                .GetUserIdByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("someId"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AuthorDTO, ProfileViewModel>(It.IsAny<AuthorDTO>()))
                .Returns(GetProfileViewModel());

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);
            controller.ControllerContext = GetFakeContext();
            var authorId = 1;

            // Act
            var result = controller.Index(authorId).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ProfileViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_WithInvalidId_Returns_RedirectToHomePageResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);
            var authorId = -1;

            // Act
            var result = controller.Index(authorId).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Edit_WithInvalidId_Returns_RedirectToHomePageResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);
            var authorId = -1;

            // Act
            var result = controller.Edit(authorId).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Edit_WithValidId_Returns_ViewResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPosts()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetCommentsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetComments()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorByUserIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthor()));


            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(identity => identity
                .GetUserIdByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("someId"));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AuthorDTO, ProfileViewModel>(It.IsAny<AuthorDTO>()))
                .Returns(GetProfileViewModel());

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);
            controller.ControllerContext = GetFakeContext();
            var authorId = 1;

            // Act
            var result = controller.Edit(authorId).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ProfileViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void EditPost_WithInvalidModel_Return_EditViewAgain()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);

            controller.ModelState.AddModelError("FirstName", "FirstNameRequired");
            var model = new ProfileViewModel { };

            // Act
            var result = controller.Edit(model).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ProfileViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void EditPost_WithValidModel_Return_RedirectToReadAction()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<UpdateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            var identityMock = new Mock<IIdentityService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<ProfileViewModel, AuthorDTO>(It.IsAny<ProfileViewModel>()))
                .Returns(GetAuthor());

            var controller = new ProfileController( identityMock.Object,
                                                    mediatorMock.Object,
                                                    mapperMock.Object);

            var uploadDataMock = new Mock<IFormFile>();
            uploadDataMock.SetupGet(upload => upload.Name).Returns("UploadData");
            uploadDataMock.SetupGet(upload => upload.Length).Returns(4096);
            uploadDataMock.Setup(upload => upload.OpenReadStream())
                .Returns(new MemoryStream());

            var binaryReaderMock = new Mock<BinaryReader>();
            byte[] image = { 0, 0, 0, 25 };
            binaryReaderMock.Setup(mapper => mapper.ReadBytes(It.IsAny<int>()))
                .Returns(image);

            var model = GetProfileViewModel();
            model.UploadedData = uploadDataMock.Object;

            // Act
            var result = controller.Edit(model).GetAwaiter().GetResult();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Profile", redirectToActionResult.ControllerName);
        }

    }
}