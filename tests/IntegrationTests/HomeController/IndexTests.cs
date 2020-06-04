using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace IntegrationTests.HomeController
{
    [Collection("Sequential")]
    public class IndexTests : IClassFixture<WebTestFixture>
    {
        public HttpClient Client { get; }

        //public IndexTests(WebTestFixture factory)
        //{   
        //    factory = factory ?? throw new ArgumentNullException(nameof(factory));
        //    Client = factory.CreateClient();
        //}

        //[Fact]
        //public async Task Index_ReturnPage_WithMainScreen()
        //{
        //    // Arrange
        //    HttpResponseMessage response = await Client.GetAsync("/");

        //    // Act
        //    response.EnsureSuccessStatusCode();
        //    var stringResponce = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    stringResponce.Should().Contain("Open Diary");

        //}
    }
}