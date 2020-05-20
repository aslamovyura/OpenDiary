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
    public class UpdatePostCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidData_ShouldUpdatePost()
        {
            // Arrange
            var post = new PostDTO
            {
                Id = 2,
                Title = "Title_new",
                Text = "Text_new",
                TopicId = 99,
                AuthorId = 99,
                Date = new DateTime(2020, 05, 01),
            };

            var command = new UpdatePostCommand { Model = post };

            // Act
            var handler = new UpdatePostCommand.UpdatePostCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Posts.Find(post.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Title.ShouldBe(command.Model.Title);
            entity.Text.ShouldBe(command.Model.Text);

            entity.TopicId.ShouldNotBe(command.Model.TopicId);
            entity.AuthorId.ShouldNotBe(command.Model.AuthorId);
            entity.Date.ShouldNotBe(command.Model.Date);
        }

        [Fact]
        public async Task Handle_GivenInvalidPostData_ThrowsException()
        {
            // Arrange
            var post = new PostDTO
            {
                Id = 99,
                Title = "Title_new",
                Text = "Text_new",
                TopicId = 2,
                AuthorId = 2,
                Date = new DateTime(2020, 05, 01),
            };

            var command = new UpdatePostCommand { Model = post };

            // Act
            var handler = new UpdatePostCommand.UpdatePostCommandHandler(Context);

            // Assert
            await Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
