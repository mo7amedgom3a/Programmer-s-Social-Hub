using MongoDB.Driver;
using UserManagementService.Models;
using UserManagementService.Interfaces;
using UserManagementService.DTOs;
using AutoMapper;
namespace UserManagementService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMapper _mapper;
        public UserRepository(IMongoDatabase database, IMapper mapper)
        {
            _users = database.GetCollection<User>("Users");
            _mapper = mapper;
        }
        public async Task<PaginatedUsers> GetUsers(int page, int pageSize)
        {
            var totalUsers = await _users.CountDocumentsAsync(user => true);
            var users = await _users.Find(user => true)
                                   
                                    .Skip((page - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();
            var usersDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserProfileDto>>(users);
            
            return new PaginatedUsers
            {
                Items = usersDto,
                TotalItems = (int)totalUsers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)totalUsers / pageSize
            };
        }
        public async Task<UserProfileDto> GetUser(string id)
        {
            var user = await _users.Find(user => user.UserId == id).FirstOrDefaultAsync();
            return _mapper.Map<UserProfileDto>(user);
        }
        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateUser(string id, UserProfileUpdateDto userDto)
        {
            var user = await _users.Find(user => user.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            var updatedUser = _mapper.Map(userDto, user);
            var result = await _users.ReplaceOneAsync(user => user.UserId == id, updatedUser);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteUser(string id)
        {
            var result = await _users.DeleteOneAsync(user => user.UserId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
        public async Task<bool> FollowUser(string userId, string targetId)
        {
            var user = await _users.Find(user => user.UserId == userId).FirstOrDefaultAsync();
            var targetUser = await _users.Find(user => user.UserId == targetId).FirstOrDefaultAsync();
            if (user == null || targetUser == null)
            {
                return false;
            }
            // Check if user is already following target
            if (user.Following.Contains(targetId))
            {
                return true;
            }
            user.Following.Add(targetId); // Add target to user's following list
            targetUser.Followers.Add(userId); // Add user to target's followers list
            var result = await _users.ReplaceOneAsync(u => u.UserId == userId, user);
            var targetResult = await _users.ReplaceOneAsync(u => u.UserId == targetId, targetUser);
            return result.IsAcknowledged && result.ModifiedCount > 0 && targetResult.IsAcknowledged && targetResult.ModifiedCount > 0;
        }
        public async Task<bool> UnfollowUser(string userId, string targetId)
        {
            var user = await _users.Find(user => user.UserId == userId).FirstOrDefaultAsync();
            var targetUser = await _users.Find(user => user.UserId == targetId).FirstOrDefaultAsync();
            if (user == null || targetUser == null)
            {
                return false;
            }
            if (!user.Following.Contains(targetId))
            {
                return true;
            }
            user.Following.Remove(targetId);
            targetUser.Followers.Remove(userId);
            var result = await _users.ReplaceOneAsync(u => u.UserId == userId, user);
            var targetResult = await _users.ReplaceOneAsync(u => u.UserId == targetId, targetUser);
            return result.IsAcknowledged && result.ModifiedCount > 0 && targetResult.IsAcknowledged && targetResult.ModifiedCount > 0;
        }
        public async Task<PaginatedUsers> GetFollowers(string userId, int page, int pageSize)
        {
            var user = await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            var followers = await _users.Find(u => user.Followers.Contains(u.UserId)).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
            var followersDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserProfileDto>>(followers);
            return new PaginatedUsers
            {
                Items = followersDto,
                TotalItems = followersDto.Count(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = followersDto.Count() / pageSize
            };
        }
        public async Task<PaginatedUsers> GetFollowing(string userId, int page, int pageSize)
        {
            var user = await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            var following = await _users.Find(u => user.Following.Contains(u.UserId)).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
            var followingDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserProfileDto>>(following);
            return new PaginatedUsers
            {
                Items = followingDto,
                TotalItems = followingDto.Count(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = followingDto.Count() / pageSize
            };
        }

    }
}