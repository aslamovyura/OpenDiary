using Application.DTO;
using AutoMapper;
using WebUI.ViewModels.Posts;

namespace Application.Mapping
{
    /// <summary>
    /// Post-PostDTO mapping rule.
    /// </summary>
    public class PostProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PostProfile()
        {
            CreateMap<PostDTO, PostViewModel>().ReverseMap();
        }
    }
}