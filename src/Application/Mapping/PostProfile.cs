using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    /// <summary>
    /// Post-PostDTO mapping rule.
    /// </summary>
    public class PostProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PostProfile()
        {
            CreateMap<Post, PostDTO>().ReverseMap()
                .ForMember(p => p.Author, opt => opt.Ignore())
                .ForMember(p => p.Topic, opt => opt.Ignore());
        }
    }
}