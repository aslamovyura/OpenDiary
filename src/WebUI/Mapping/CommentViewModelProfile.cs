using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// Comment-CommentDTO mapping rule.
    /// </summary>
    public class CommentViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CommentViewModelProfile()
        {
            CreateMap<CommentDTO, CommentViewModel>()
                .ForMember(model => model.Author, opt => opt.Ignore())
                .ForMember(model => model.AuthorAvatar, opt => opt.Ignore())
                .ForMember(model => model.Age, opt => opt.Ignore())
                .ForMember(model => model.AgeUnits, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}