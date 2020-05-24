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
            CreateMap<CommentDTO, CommentViewModel>().ReverseMap();
        }
    }
}