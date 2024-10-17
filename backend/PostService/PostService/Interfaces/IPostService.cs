using PostService.DTOs;

namespace PostService.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(string id);
        Task CreatePostAsync(CreatePostDto postDto);
        Task UpdatePostAsync(string id, UpdatePostDto postDto);
        Task DeletePostAsync(string id);
        Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId);
        Task<PaginatedPostsDto> GetPaginatedPostsAsync(int page, int pageSize);
        Task<PaginatedPostsDto> GetPaginatedPostsByIdAsync(string userId, int page, int pageSize);
    }
}
