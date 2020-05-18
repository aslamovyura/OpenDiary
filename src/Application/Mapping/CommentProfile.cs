using System;
using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    /// <summary>
    /// Comment-CommentDTO mapping rule.
    /// </summary>
    public class CommentProfile : Profile
    {
        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}