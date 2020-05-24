using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// Post-PostDTO mapping rule.
    /// </summary>
    public class PostViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PostViewModelProfile()
        {
            CreateMap<PostDTO, PostViewModel>().ReverseMap();
        }
    }
}