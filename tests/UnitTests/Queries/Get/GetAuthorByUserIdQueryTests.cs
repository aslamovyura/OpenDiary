using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetAuthorByUserIdQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handle_GivenValidUserId_ReturnsAuthorDTO()
        {
            // Arrange
            var author = new AuthorDTO
            {
                Id = 1,
                UserId = "QWERTY1234567890_One",
                FirstName = "FirstName_One",
                LastName = "LastName_One",
                BirthDate = new DateTime(1988, 01, 01),
            };

            var query = new GetAuthorByUserIdQuery { UserId = "QWERTY1234567890_One" };

            // Act
            var handler = new GetAuthorByUserIdQuery.GetAuthorByUserIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<AuthorDTO>();
            result.ShouldNotBeNull();

            result.Id.ShouldBe(author.Id);
            result.UserId.ShouldBe(author.UserId);
            result.FirstName.ShouldBe(author.FirstName);
            result.LastName.ShouldBe(author.LastName);
            result.BirthDate.ShouldBe(author.BirthDate);
        }

        [Fact]
        public async Task Handle_GivenInvalidUserId_ReturnsNull()
        {
            // Arrange
            var query = new GetAuthorByUserIdQuery { UserId = "InvalidUserId" };

            // Act
            var handler = new GetAuthorByUserIdQuery.GetAuthorByUserIdQueryHandler(Context, Mapper);
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }
    }
}
