using AutoMapper;
using PostService.DTOs;
using PostService.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.CommentIds)).ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CreateCommentDto>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CodeSection)).ReverseMap();
        CreateMap<Comment, UpdateCommentDto>()
            .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CodeSection)).ReverseMap();
        CreateMap<CreatePostDto, Post>();
        CreateMap<UpdatePostDto, Post>();

    }
}
