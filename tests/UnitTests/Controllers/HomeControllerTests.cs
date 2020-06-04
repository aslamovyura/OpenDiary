using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebUI.Controllers;
using Xunit;

namespace UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Return_ViewResult()
        {
            // Arrange
            var mock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}