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
    public class GetPostsQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ReturnsPostDTOCollection()
        {
            // Arrange
            var query = new GetPostsQuery();

            // Act
            var handler = new GetPostsQuery.GetPostsQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<PostDTO>>();
            result.ShouldNotBeNull();
        }
    }
}