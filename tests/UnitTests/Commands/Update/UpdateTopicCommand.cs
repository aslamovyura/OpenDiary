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
    public class UpdateTopicCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidData_ShouldUpdateTopic()
        {
            // Arrange
            var topic = new TopicDTO
            {
                Id = 1,
                Text = "Topic_new",
            };

            var command = new UpdateTopicCommand { Model = topic };

            // Act
            var handler = new UpdateTopicCommand.UpdateTopicCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Topics.Find(topic.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Text.ShouldBe(command.Model.Text);
        }

        [Fact]
        public async Task Handle_GivenInvalidTopicData_ThrowsException()
        {
            // Arrange
            var topic = new TopicDTO
            {
                Id = 99,
                Text = "Topic_new",
            };

            var command = new UpdateTopicCommand { Model = topic };

            // Act
            var handler = new UpdateTopicCommand.UpdateTopicCommandHandler(Context);

            // Assert
            await Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
