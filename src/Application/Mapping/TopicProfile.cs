using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    /// <summary>
    /// Topic-TopicDTO mapping rule.
    /// </summary>
    public class TopicProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public TopicProfile()
        {
            //CreateMap<Topic, TopicDTO>().ReverseMap();
            CreateMap<Topic, TopicDTO>()
                .ReverseMap()
                .ForPath(s => s.Posts, opt => opt.Ignore());
        }
    }
}