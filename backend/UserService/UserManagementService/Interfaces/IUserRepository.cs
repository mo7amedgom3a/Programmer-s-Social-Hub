using UserManagementService.Models;
using UserManagementService.DTOs;
namespace UserManagementService.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginatedUsers> GetUsers(int page, int pageSize);
        Task<UserProfileDto> GetUser(string id);
        Task<User> CreateUser(User user);
        Task<bool> UpdateUser(string id, UserProfileUpdateDto user);
        Task<bool> DeleteUser(string id);
        Task<bool> FollowUser(string userId, string targetId);
        Task<bool> UnfollowUser(string userId, string targetId);

        Task<PaginatedUsers> GetFollowers(string userId, int page, int pageSize);
        Task<PaginatedUsers> GetFollowing(string userId, int page, int pageSize);
       
    }
}