using UserManagementService.DTOs;
using UserManagementService.Models;

namespace UserManagementService.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedUsers> GetAllUsersAsync(int page, int pageSize);
        Task<UserProfileDto> GetUserByIdAsync(string id);
        Task CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(string id, UserProfileUpdateDto updatedUser);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> FollowUserAsync(string userId, string targetId);
        Task<bool> UnfollowUserAsync(string userId, string targetId);
        Task<PaginatedUsers> GetFollowersAsync(string userId, int page, int pageSize);
        Task<PaginatedUsers> GetFollowingAsync(string userId, int page, int pageSize);

       
    }
}
