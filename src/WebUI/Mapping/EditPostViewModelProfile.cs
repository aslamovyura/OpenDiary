using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// EditPostViewModel-PostDTO mapping rule.
    /// </summary>
    public class EditPostViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public EditPostViewModelProfile()
        {
            CreateMap<PostDTO, EditPostViewModel>().ReverseMap();
        }
    }
}