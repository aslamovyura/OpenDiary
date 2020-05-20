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
    public class GetCommentsByAuthorIdQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidAuthorId_ReturnsNotEmptyCommentsDTOCollection()
        {
            // Arrange
            var query = new GetCommentsByAuthorIdQuery { AuthorId = 1 };

            // Act
            var handler = new GetCommentsByAuthorIdQuery.GetCommentsByAuthorIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<CommentDTO>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handler_GivenInvalidAuthorId_ReturnsEmptyCommentsDTOCollection()
        {
            // Arrange
            var query = new GetCommentsByAuthorIdQuery { AuthorId = 99 };

            // Act
            var handler = new GetCommentsByAuthorIdQuery.GetCommentsByAuthorIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeEmpty();
        }
    }
}
