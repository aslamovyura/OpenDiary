using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Delete;
using Xunit;
using Shouldly;
using Application.Exceptions;

namespace UnitTests.Commands.Delete
{
    public class DeleteTopicCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidTopicId_ShouldRemovePost()
        {
            // Arrange
            var validTopicId = 1;

            // Act
            var command = new DeleteTopicCommand { Id = validTopicId };
            var handler = new DeleteTopicCommand.DeleteTopicCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var entity = Context.Topics.Find(command.Id);
            entity.ShouldBeNull();
        }

        [Fact]
        public void Handler_GivenInvalidTopicId_ThrowsException()
        {
            // Arrange
            var invalidTopicId = 99;

            // Act
            var command = new DeleteTopicCommand { Id = invalidTopicId };
            var handler = new DeleteTopicCommand.DeleteTopicCommandHandler(Context);

            // Assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}