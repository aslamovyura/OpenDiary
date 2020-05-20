using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Commands.Create
{
    public class CreatePostCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ShouldAddPost()
        {
            // Arrange
            var post = new PostDTO
            {
                Id = 7,
                Title = "Title_test",
                Text = "Text_test",
                TopicId = 1,
                AuthorId = 1,
                Date = new DateTime(2020, 03, 01),
            };

            var command = new CreatePostCommand { Model = post };

            // Act
            var handler = new CreatePostCommand.CreatePostCommandHandler(Context, Mapper);
            await handler.Handle(command, CancellationToken.None);
            var entity = Context.Posts.Find(post.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Id.ShouldBe(command.Model.Id);
            entity.Title.ShouldBe(command.Model.Title);
            entity.Text.ShouldBe(command.Model.Text);
            entity.TopicId.ShouldBe(command.Model.TopicId);
            entity.AuthorId.ShouldBe(command.Model.AuthorId);
            entity.Date.ShouldBe(command.Model.Date);
        }
    }
}