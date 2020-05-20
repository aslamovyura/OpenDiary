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
    public class GetPostsByAuthorIdQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidAuthorId_ReturnsNotEmptyPostsDTOCollection()
        {
            // Arrange
            var query = new GetPostsByAuthorIdQuery { AuthorId = 1 };

            // Act
            var handler = new GetPostsByAuthorIdQuery.GetPostsByAuthorIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<PostDTO>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handler_GivenInvalidAuthorId_ReturnsEmptyCommentsDTOCollection()
        {
            // Arrange
            var query = new GetPostsByAuthorIdQuery { AuthorId = 99 };

            // Act
            var handler = new GetPostsByAuthorIdQuery.GetPostsByAuthorIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeEmpty();
        }
    }
}
