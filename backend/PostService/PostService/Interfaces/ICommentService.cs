using PostService.DTOs;

namespace PostService.Interfaces
{
    public interface ICommentService
    
    {
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId);
        Task AddCommentToPostAsync(string postId, CreateCommentDto comment);
        Task UpdateCommentAsync(string postId,  UpdateCommentDto comment);
        Task DeleteCommentAsync(string postId, string commentId);
    }
}