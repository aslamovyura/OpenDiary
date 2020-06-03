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
            CreateMap<AuthorDTO, ProfileViewModel>()
                .ForMember(model => model.Age, opt => opt.Ignore())
                .ForMember(model => model.CurrentReaderId, opt => opt.Ignore())
                .ForMember(model => model.TotalCommentsNumber, opt => opt.Ignore())
                .ForMember(model => model.TotalPostsNumber, opt => opt.Ignore())
                .ForMember(model => model.UploadedData, opt => opt.Ignore())
                .ForMember(model => model.Posts, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dto => dto.UserId, opt => opt.Ignore());
        }
    }
}