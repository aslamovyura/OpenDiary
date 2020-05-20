using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetCommentsByPostIdQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidPostId_ReturnsNotEmptyCommentsDTOCollection()
        {
            // Arrange
            var query = new GetCommentsByPostIdQuery { PostId = 1 };

            // Act
            var handler = new GetCommentsByPostIdQuery.GetCommentsByPostIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<CommentDTO>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handler_GivenInvalidPostId_ReturnsEmptyCommentsDTOCollection()
        {
            // Arrange
            var query = new GetCommentsByPostIdQuery { PostId = 99 };

            // Act
            var handler = new GetCommentsByPostIdQuery.GetCommentsByPostIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeEmpty();
        }
    }
}
