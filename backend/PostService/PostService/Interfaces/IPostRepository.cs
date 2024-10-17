using MongoDB.Driver;
using PostService.DTOs;
using PostService.Models;

namespace PostService.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(string id);
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task UpdatePostAsyncById(string id, UpdateDefinition<Post> update);
        Task DeletePostAsync(string id);
        
        Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId);
        Task<PaginatedPostsDto> GetPaginatedPostsAsync(int page, int pageSize);
        Task<PaginatedPostsDto> GetPaginatedPostsByIdAsync(string userId, int page, int pageSize);
    }
}
