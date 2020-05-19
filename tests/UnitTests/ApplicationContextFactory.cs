using System;
using System.Collections.Generic;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    /// <summary>
    /// Factory to fill datababe with seed test data.
    /// </summary>
    public static class ApplicationContextFactory
    {
        /// <summary>
        /// Create database.
        /// </summary>
        /// <returns>Context of sample database.</returns>
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
        }

        /// <summary>
        /// Fill database with seed sample data.
        /// </summary>
        /// <param name="context">Context of sample database.</param>
        public static void SeedSampleData(ApplicationDbContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    UserId = "QWERTY1234567890_One",
                    FirstName = "FirstName_One",
                    LastName = "LastName_One",
                    BirthDate = new DateTime(1988, 01, 01),
                },

                new Author
                {
                    Id = 2,
                    UserId = "QWERTY1234567890_Two",
                    FirstName = "FirstName_Two",
                    LastName = "LastName_Two",
                    BirthDate = new DateTime(1989, 01, 01),
                },

                new Author
                {
                    Id = 3,
                    UserId = "QWERTY1234567890_Three",
                    FirstName = "FirstName_Three",
                    LastName = "LastName_Three",
                    BirthDate = new DateTime(1990, 01, 01),
                },
            };

            var topics = new List<Topic>
            {
                new Topic
                {
                    Id = 1,
                    Text = "Topic_One",
                },

                new Topic
                {
                    Id = 2,
                    Text = "Topic_Two",
                }
            };

            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    AuthorId = 1,
                    Date = new DateTime(2020, 01, 01),
                    Title = "Title_One",
                    Text = "Test_Two",
                    TopicId = 1,
                },

                new Post
                {
                    Id = 2,
                    AuthorId = 1,
                    Date = new DateTime(2020, 01, 02),
                    Title = "Title_Two",
                    Text = "Test_Two",
                    TopicId = 2,
                },

                new Post
                {
                    Id = 3,
                    AuthorId = 2,
                    Date = new DateTime(2020, 01, 03),
                    Title = "Title_Three",
                    Text = "Test_Three",
                    TopicId = 1,
                },

                new Post
                {
                    Id = 4,
                    AuthorId = 2,
                    Date = new DateTime(2020, 01, 04),
                    Title = "Title_Four",
                    Text = "Test_Four",
                    TopicId = 2,
                },

                new Post
                {
                    Id = 5,
                    AuthorId = 3,
                    Date = new DateTime(2020, 01, 05),
                    Title = "Title_Five",
                    Text = "Test_Five",
                    TopicId = 1,
                },

                new Post
                {
                    Id = 6,
                    AuthorId = 1,
                    Date = new DateTime(2020, 01, 06),
                    Title = "Title_Six",
                    Text = "Test_Six",
                    TopicId = 2,
                },
            };

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    AuthorId = 1,
                    Date = new DateTime(2020, 02, 01),
                    PostId = 4,
                    Text = "Comment_One",
                },

                new Comment
                {
                    Id = 2,
                    AuthorId = 1,
                    Date = new DateTime(2020, 02, 02),
                    PostId = 5,
                    Text = "Comment_Two",
                },

                new Comment
                {
                    Id = 3,
                    AuthorId = 2,
                    Date = new DateTime(2020, 02, 03),
                    PostId = 5,
                    Text = "Comment_Three",
                },

                new Comment
                {
                    Id = 4,
                    AuthorId = 2,
                    Date = new DateTime(2020, 02, 04),
                    PostId = 2,
                    Text = "Comment_Four",
                },
            };

            context.Authors.AddRange(authors);
            context.Posts.AddRange(posts);
            context.Topics.AddRange(topics);
            context.Comments.AddRange(comments);

            context.SaveChanges();
        }

        /// <summary>
        /// Destroy database.
        /// </summary>
        /// <param name="context">Context of sample database.</param>
        public static void Destroy(ApplicationDbContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}