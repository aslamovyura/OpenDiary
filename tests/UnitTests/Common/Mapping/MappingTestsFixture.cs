using Application.Mapping;
using AutoMapper;

namespace UnitTests.Common.Mapping
{
    public class MappingTestsFixture
    {
        /// <summary>
        /// Key/value configuration.
        /// </summary>
        public IConfigurationProvider ConfigurationProvider { get; }

        /// <summary>
        /// Object mapper.
        /// </summary>
        public IMapper Mapper { get; }

        public MappingTestsFixture()
        {
            ConfigurationProvider = new MapperConfiguration(config =>
            {
                // Application
                config.AddProfile<AuthorProfile>();
                config.AddProfile<CommentProfile>();
                config.AddProfile<PostProfile>();
                config.AddProfile<TopicProfile>();

                // WebUI
                config.AddProfile<AuthorViewModelProfile>();
                config.AddProfile<CommentViewModelProfile>();
                config.AddProfile<EditPostViewModelProfile>();
                config.AddProfile<PostViewModelProfile>();
                config.AddProfile<ProfileViewModelProfile>();
                config.AddProfile<TopicViewModelProfile>();
            });

            Mapper = ConfigurationProvider.CreateMapper();
        } 
    }
}
