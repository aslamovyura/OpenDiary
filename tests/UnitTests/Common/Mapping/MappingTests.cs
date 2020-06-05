using System;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using WebUI.ViewModels;
using WebUI.ViewModels.Posts;
using WebUI.ViewModels.Profile;
using Xunit;

namespace UnitTests.Common.Mapping
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)

        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void Mapper_ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Author), typeof(AuthorDTO))]
        [InlineData(typeof(Comment), typeof(CommentDTO))]
        [InlineData(typeof(Post), typeof(PostDTO))]
        [InlineData(typeof(Topic), typeof(TopicDTO))]
        [InlineData(typeof(AuthorViewModel), typeof(AuthorDTO))]
        [InlineData(typeof(CommentDTO), typeof(CommentViewModel))]
        [InlineData(typeof(PostDTO), typeof(EditPostViewModel))]
        [InlineData(typeof(PostDTO), typeof(PostViewModel))]
        [InlineData(typeof(AuthorDTO), typeof(ProfileViewModel))]
        [InlineData(typeof(TopicDTO), typeof(TopicViewModel))]
        public void Mapper_ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);
            _mapper.Map(instance, source, destination);
        }
    }
}