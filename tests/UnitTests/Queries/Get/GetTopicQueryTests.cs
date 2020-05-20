using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetTopicQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handle_GivenValidId_ReturnsTopicDTO()
        {
            // Arrange
            var topic = new TopicDTO
            {
                Id = 1,
                Text = "Topic_One",
            };

            var query = new GetTopicQuery { Id = 1 };

            // Act
            var handler = new GetTopicQuery.GetTopicQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<TopicDTO>();
            result.ShouldNotBeNull();

            result.Id.ShouldBe(topic.Id);
            result.Text.ShouldBe(topic.Text);
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ReturnsNull()
        {
            // Arrange
            var query = new GetCommentQuery { Id = 99 };

            // Act
            var handler = new GetCommentQuery.GetCommentQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }
    }
}