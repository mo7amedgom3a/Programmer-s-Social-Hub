using Microsoft.AspNetCore.Identity;
using SecurityServiceBackend.DTOs;
using SecurityServiceBackend.Interfaces;
using SecurityServiceBackend.Models;
namespace SecurityServiceBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
        {
            try
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                var result = await _userManager.CreateAsync(user);
                if (result != null && result.Succeeded)
                {
                    Console.WriteLine($"User {user.UserName} created");
                    return user;
                }
                else
                {
                    Console.WriteLine($"Failed to create user {result.Errors}");
                    Console.WriteLine($"Failed to create user {user.UserName}");
                }
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

    }
}