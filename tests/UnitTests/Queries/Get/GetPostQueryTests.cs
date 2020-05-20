using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetPostQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handle_GivenValidId_ReturnsPostDTO()
        {
            // Arrange
            var post = new PostDTO
            {
                Id = 1,
                AuthorId = 1,
                Date = new DateTime(2020, 01, 01),
                Title = "Title_One",
                Text = "Test_Two",
                TopicId = 1,
            };

            var query = new GetPostQuery { Id = 1 };

            // Act
            var handler = new GetPostQuery.GetPostQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<PostDTO>();
            result.ShouldNotBeNull();

            result.Id.ShouldBe(post.Id);
            result.AuthorId.ShouldBe(post.AuthorId);
            result.Date.ShouldBe(post.Date);
            result.Title.ShouldBe(post.Title);
            result.Text.ShouldBe(post.Text);
            result.TopicId.ShouldBe(post.TopicId);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ReturnsNull()
        {
            // Arrange
            var query = new GetPostQuery { Id = 99 };

            // Act
            var handler = new GetPostQuery.GetPostQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }
    }
}