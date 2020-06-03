using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// AuthorDTO-EditUserViewModel mapping rule.
    /// </summary>
    public class AuthorViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public AuthorViewModelProfile()
        {
            CreateMap<AuthorViewModel, AuthorDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.AuthorId))
                .ForMember(dto => dto.About, opt => opt.Ignore())
                .ForMember(dto => dto.Hobbies, opt => opt.Ignore())
                .ForMember(dto => dto.Profession, opt => opt.Ignore())
                .ReverseMap()
                .ForPath(model => model.AuthorId, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(model => model.PostsNumber, opt => opt.Ignore())
                .ForMember(model => model.CommentsNumber, opt => opt.Ignore());
        }
    }
}