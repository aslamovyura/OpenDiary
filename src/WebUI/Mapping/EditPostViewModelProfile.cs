using Application.DTO;
using AutoMapper;
using WebUI.ViewModels.Posts;

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
            CreateMap<PostDTO, EditPostViewModel>()
                .ForMember(model => model.Topics, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dto => dto.Date, opt => opt.Ignore())
                .ForMember(dto => dto.AuthorId, opt => opt.Ignore())
                .ForMember(dto => dto.Author, opt => opt.Ignore());
        }
    }
}