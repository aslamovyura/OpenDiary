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
    public class GetTopicsQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ReturnsPostDTOCollection()
        {
            // Arrange
            var query = new GetTopicsQuery();

            // Act
            var handler = new GetTopicsQuery.GetTopicsQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<TopicDTO>>();
            result.ShouldNotBeNull();
        }
    }
}