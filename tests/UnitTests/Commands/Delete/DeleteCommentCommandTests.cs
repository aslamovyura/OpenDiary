using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Delete;
using Xunit;
using Shouldly;
using Application.Exceptions;

namespace UnitTests.Commands.Delete
{
    public class DeleteCommentCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidCommentId_ShouldRemoveComment()
        {
            // Arrange
            var validCommentId = 3;

            // Act
            var command = new DeleteCommentCommand { Id = validCommentId };
            var handler = new DeleteCommentCommand.DeleteCommentCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var entity = Context.Comments.Find(command.Id);
            entity.ShouldBeNull();
        }

        [Fact]
        public void Handler_GivenInvalidCommentId_ThrowsException()
        {
            // Arrange
            var invalidCommentId = 99;

            // Act
            var command = new DeleteCommentCommand{ Id = invalidCommentId };
            var handler = new DeleteCommentCommand.DeleteCommentCommandHandler(Context);

            // Assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}