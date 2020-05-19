using System;
using Application.Mapping;
using AutoMapper;
using Infrastructure.Persistence;

namespace UnitTests
{
    /// <summary>
    /// Base tests fixture.
    /// </summary>
    public class BaseTestsFixture : IDisposable
    {
        /// <summary>
        /// Define base tests fixture.
        /// </summary>
        public BaseTestsFixture()
        {
            Context = ApplicationContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuthorProfile>();
                cfg.AddProfile<TopicProfile>();
                cfg.AddProfile<PostProfile>();
                cfg.AddProfile<CommentProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        /// <summary>
        /// Context of sample database.
        /// </summary>
        public ApplicationDbContext Context { get; }

        /// <summary>
        /// AutoMapper для DTO и основных моделей.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Разрушить контекст.
        /// </summary>
        public void Dispose()
        {
            ApplicationContextFactory.Destroy(Context);
        }
    }
}