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
    public class GetAuthorsQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ReturnsAuthorDTOCollection()
        {
            // Arrange
            var query = new GetAuthorsQuery();

            // Act
            var handler = new GetAuthorsQuery.GetAuthorsQueryHandler(Context, Mapper);

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<AuthorDTO>>();
            result.ShouldNotBeNull();
        }
    }
}