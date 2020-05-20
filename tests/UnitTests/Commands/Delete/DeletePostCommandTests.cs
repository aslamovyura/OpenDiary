using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Delete;
using Xunit;
using Shouldly;
using Application.Exceptions;

namespace UnitTests.Commands.Delete
{
    public class DeletePostCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidPostId_ShouldRemovePost()
        {
            // Arrange
            var validPostId = 2;

            // Act
            var command = new DeletePostCommand { Id = validPostId };
            var handler = new DeletePostCommand.DeletePostCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var entity = Context.Posts.Find(command.Id);
            entity.ShouldBeNull();
        }

        [Fact]
        public void Handler_GivenInvalidPostId_ThrowsException()
        {
            // Arrange
            var invalidPostId = 99;

            // Act
            var command = new DeletePostCommand{ Id = invalidPostId };
            var handler = new DeletePostCommand.DeletePostCommandHandler(Context);

            // Assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}