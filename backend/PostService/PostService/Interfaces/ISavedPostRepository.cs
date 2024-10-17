using PostService.Models;

namespace PostService.Interfaces
{
    public interface ISavedPostRepository
    {
        Task<IEnumerable<SavedPost>> GetSavedPostsByUserIdAsync(string userId);
        Task SavePostAsync(string postId, string userId);
        Task UnsavePostAsync(string postId, string userId);
        Task RemoveSavedPostAsync(string userId, string postId);
    }
}
