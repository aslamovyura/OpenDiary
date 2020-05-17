using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    /// <summary>
    /// Author-AuthorDTO mapping rule.
    /// </summary>
    public class AuthorProfile : Profile
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            //CreateMap<Author, AuthorDTO>().ReverseMap()
            //    .ForMember(p => p.Comments, opt => opt.Ignore())
            //    .ForMember(p => p.Posts, opt => opt.Ignore());
        }
    }
}