using MongoDB.Driver;
using PostService.DTOs;
using PostService.Models;

namespace PostService.Interfaces
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetLikesByPostIdAsync(string postId);
        Task LikePostAsync(string postId, string userId);
        Task DislikePostAsync(string postId, string userId);
    }
}