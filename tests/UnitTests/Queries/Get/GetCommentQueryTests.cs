using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetCommentQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handle_GivenValidId_ReturnsCommentDTO()
        {
            // Arrange
            var comment = new CommentDTO
            {
                Id = 1,
                AuthorId = 1,
                Date = new DateTime(2020, 02, 01),
                PostId = 4,
                Text = "Comment_One",
            };

            var query = new GetCommentQuery { Id = 1 };

            // Act
            var handler = new GetCommentQuery.GetCommentQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<CommentDTO>();
            result.ShouldNotBeNull();

            result.Id.ShouldBe(comment.Id);
            result.AuthorId.ShouldBe(comment.AuthorId);
            result.Date.ShouldBe(comment.Date);
            result.PostId.ShouldBe(comment.PostId);
            result.Text.ShouldBe(comment.Text);
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