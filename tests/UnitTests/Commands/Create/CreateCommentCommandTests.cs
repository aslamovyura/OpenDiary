using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Commands.Create;
using Application.DTO;
using Shouldly;
using Xunit;

namespace UnitTests.Commands.Create
{
    public class CreateCommentCommandTests : BaseTestsFixture
    {
        [Fact]
        public async Task Handler_ShouldAddComment()
        {
            // Arrange
            var comment = new CommentDTO
            {
                Id = 7,
                PostId = 1,
                Text = "Comment_test",
                AuthorId = 1,
                Date = new DateTime(2020, 03, 01),
            };

            var command = new CreateCommentCommand { Model = comment };

            // Act
            var handler = new CreateCommentCommand.CreateCommentCommandHandler(Mapper, Context);
            await handler.Handle(command, CancellationToken.None);
            var entity = Context.Comments.Find(comment.Id);

            // Assert
            entity.ShouldNotBeNull();

            entity.Id.ShouldBe(command.Model.Id);
            entity.PostId.ShouldBe(command.Model.PostId);
            entity.Text.ShouldBe(command.Model.Text);
            entity.AuthorId.ShouldBe(command.Model.AuthorId);
            entity.Date.ShouldBe(command.Model.Date);
        }
    }
}