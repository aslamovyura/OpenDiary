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
    public class UpdateCommentCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_GivenValidData_ShouldUpdateComment()
        {
            // Arrange
            var comment = new CommentDTO
            {
                Id = 1,
                PostId = 99,
                Text = "Comment_new",
                AuthorId = 99,
                Date = new DateTime(2020, 05, 01),
            };

            var command = new UpdateCommentCommand { Model = comment };

            // Act
            var handler = new UpdateCommentCommand.UpdateCommentCommandHandler(Context);
            await handler.Handle(command, CancellationToken.None);

            var entity = Context.Comments.Find(comment.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Text.ShouldBe(command.Model.Text);

            entity.PostId.ShouldNotBe(command.Model.PostId);
            entity.AuthorId.ShouldNotBe(command.Model.AuthorId);
            entity.Date.ShouldNotBe(command.Model.Date);
        }

        [Fact]
        public async Task Handle_GivenInvalidCommentData_ThrowsException()
        {
            // Arrange
            var comment = new CommentDTO
            {
                Id = 99,
                PostId = 99,
                Text = "Comment_test",
                AuthorId = 99,
                Date = new DateTime(2020, 05, 01),
            };

            var command = new UpdateCommentCommand { Model = comment };

            // Act
            var handler = new UpdateCommentCommand.UpdateCommentCommandHandler(Context);

            // Assert
            await Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
