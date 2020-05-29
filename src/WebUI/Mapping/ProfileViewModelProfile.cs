using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// AuthorDTO-ProfileViewModel mapping rule.
    /// </summary>
    public class ProfileViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ProfileViewModelProfile()
        {
            CreateMap<ProfileViewModel, AuthorDTO>().ReverseMap();
        }
    }
}