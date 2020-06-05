using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using CustomIdentityApp.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUI.ViewModels;
using Xunit;

namespace UnitTests.Controllers
{
    public class AuthorsControllerTests : ControllerTestsFixture
    {
        [Fact]
        public void Index_Return_ViewResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetAuthorsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetAuthors()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetPostsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetPosts()));

            mediatorMock.Setup(mediator => mediator
                .Send(It.IsAny<GetCommentsByAuthorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetComments()));

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(identity => identity
                .GetEmailByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("some@email.com"));

            var context = GetFakeContext();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<AuthorDTO>, ICollection<AuthorViewModel>>(It.IsAny<IEnumerable<AuthorDTO>>()))
                .Returns(GetAuthorVieModels());

            var controller = new AuthorsController( identityMock.Object,
                                                    mediatorMock.Object,                                 
                                                    mapperMock.Object);
            controller.ControllerContext = context;

            // Act
            var result = controller.Index().GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var models = Assert.IsAssignableFrom<AuthorsViewModel>(viewResult.ViewData.Model);
        }
    }
}