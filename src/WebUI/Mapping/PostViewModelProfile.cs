using Application.DTO;
using AutoMapper;
using WebUI.ViewModels.Posts;

namespace Application.Mapping
{
    /// <summary>
    /// PostViewModel-PostDTO mapping rule.
    /// </summary>
    public class PostViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PostViewModelProfile()
        {
            CreateMap<PostDTO, PostViewModel>()
                .ForMember(model => model.AuthorAvatar, opt => opt.Ignore())
                .ForMember(model => model.CurrentReaderId, opt => opt.Ignore())
                .ForMember(model => model.Comments, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}