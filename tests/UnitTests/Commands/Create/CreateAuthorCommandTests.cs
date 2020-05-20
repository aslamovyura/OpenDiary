using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Commands.Create
{
    public class CreateAuthorCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ShouldAddAuthor()
        {
            // Arrange
            var author = new AuthorDTO
            {
                Id = 4,
                UserId = "QWERTY1234567890_test",
                FirstName = "FirstName_test",
                LastName = "LastName_test",
                BirthDate = new DateTime(2004, 01, 01),
                Email = "test@gmail.com",
            };

            var command = new CreateAuthorCommand { Model = author };

            // Act
            var handler = new CreateAuthorCommand.CreateAuthorCommandHandler(Context, Mapper);
            await handler.Handle(command, CancellationToken.None);
            var entity = Context.Authors.Find(author.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Id.ShouldBe(command.Model.Id);
            entity.UserId.ShouldBe(command.Model.UserId);
            entity.FirstName.ShouldBe(command.Model.FirstName);
            entity.LastName.ShouldBe(command.Model.LastName);
            entity.BirthDate.ShouldBe(command.Model.BirthDate);
        }
    }
}