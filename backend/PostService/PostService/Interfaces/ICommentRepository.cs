using PostService.DTOs;
using PostService.Models;

namespace PostService.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId);
        Task<CommentDto> GetCommentByIdAsync(string commentId);
        Task AddCommentAsync(string postId, CreateCommentDto comment);
        Task UpdateCommentAsync(string postId, Comment comment);
        Task DeleteCommentAsync(string postId, string commentId);
    }
}
