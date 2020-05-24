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
                .ReverseMap()
                .ForPath(model => model.AuthorId, opt => opt.MapFrom(dto => dto.Id));
        }
    }
}