using Application.DTO;
using AutoMapper;
using WebUI.ViewModels;

namespace Application.Mapping
{
    /// <summary>
    /// TopicDTO-TopicViewModel mapping rule.
    /// </summary>
    public class TopicViewModelProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public TopicViewModelProfile()
        {
            CreateMap<TopicDTO, TopicViewModel>().ReverseMap();
        }
    }
}