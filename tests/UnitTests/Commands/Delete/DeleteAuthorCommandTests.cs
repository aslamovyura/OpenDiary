using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Delete;
using Xunit;
using Shouldly;
using Application.Exceptions;

namespace UnitTests.Commands.Delete
{
    public class DeleteAuthorCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidAuthorId_ShouldRemoveAuthor()
        {
            // Arrange
            var validAuthorId = 3;

            // Act
            var command = new DeleteAuthorCommand { Id = validAuthorId };
            var handler = new DeleteAuthorCommand.DeleteAuthorCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var entity = Context.Authors.Find(command.Id);
            entity.ShouldBeNull();
        }

        [Fact]
        public void Handler_GivenInvalidAuthorId_ThrowsException()
        {
            // Arrange
            var invalidAuthorId = 99;

            // Act
            var command = new DeleteAuthorCommand{ Id = invalidAuthorId };
            var handler = new DeleteAuthorCommand.DeleteAuthorCommandHandler(Context);

            // Assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}