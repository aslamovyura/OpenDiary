using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Update;
using Application.DTO;
using Application.Exceptions;
using Shouldly;
using Xunit;

namespace UnitTests.Commands.Update
{
    public class UpdateAuthorCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidData_ShouldUpdateAuthor()
        {
            // Arrange
            var author = new AuthorDTO
            {
                Id = 2,
                UserId = "QWERTY1234567890_new",
                FirstName = "FirstName_new",
                LastName = "LastName_new",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "new@gmail.com",
            };

            var command = new UpdateAuthorCommand { Model = author };

            // Act
            var handler = new UpdateAuthorCommand.UpdateAuthorCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Authors.Find(author.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.FirstName.ShouldBe(command.Model.FirstName);
            entity.LastName.ShouldBe(command.Model.LastName);
            entity.BirthDate.ShouldBe(command.Model.BirthDate);

            entity.UserId.ShouldNotBe(command.Model.UserId);
        }

        [Fact]
        public async Task Handle_GivenInvalidAuthorData_ThrowsException()
        {
            // Arrange
            var author = new AuthorDTO
            {
                Id = 99,
                UserId = "QWERTY1234567890_new",
                FirstName = "FirstName_new",
                LastName = "LastName_new",
                BirthDate = new DateTime(2000, 01, 01),
                Email = "new@gmail.com",
            };

            var command = new UpdateAuthorCommand { Model = author };

            // Act
            var handler = new UpdateAuthorCommand.UpdateAuthorCommandHandler(Context);

            // Assert
            await Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
