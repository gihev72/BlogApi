using AutoMapper;
using BlogApp.Dto;
using BlogApp.Models;

namespace BlogApp.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Blog,BlogDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
            CreateMap<AuthorDto, Author>();
            CreateMap<CreateAuthorDto , Author>();
            CreateMap<CreateUserDto , User>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<Author, AuthorDto>();
            CreateMap<User, UserDto>();
            CreateMap<Comment, CommentDto>();
        }
    }
}
