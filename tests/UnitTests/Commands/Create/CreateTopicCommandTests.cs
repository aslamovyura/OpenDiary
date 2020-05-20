using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Commands.Create
{
    public class CreateTopicCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ShouldAddTopic()
        {
            // Arrange
            var topic = new TopicDTO
            {
                Id = 7,
                Text = "Topic_test",
            };

            var command = new CreateTopicCommand { Model = topic };

            // Act
            var handler = new CreateTopicCommand.CreateTopicCommandHandler(Context, Mapper);
            await handler.Handle(command, CancellationToken.None);
            var entity = Context.Topics.Find(topic.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Id.ShouldBe(command.Model.Id);
            entity.Text.ShouldBe(command.Model.Text);
        }
    }
}