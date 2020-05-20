using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Queries.Get;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Queries.Get
{
    public class GetAuthorQueryTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handle_GivenValidId_ReturnsAuthorDTO()
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

            var query = new GetAuthorQuery { Id = 1 };

            // Act
            var handler = new GetAuthorQuery.GetAuthorQueryHandler(Context, Mapper);
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
        public async Task Handle_GivenInvalidId_ReturnsNull()
        {
            // Arrange
            var query = new GetAuthorQuery { Id = 99 };

            // Act
            var handler = new GetAuthorQuery.GetAuthorQueryHandler(Context, Mapper);

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }
    }
}
