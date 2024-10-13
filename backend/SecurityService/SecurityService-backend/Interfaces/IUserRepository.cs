using Microsoft.AspNetCore.Identity;
using SecurityServiceBackend.Models;

namespace SecurityServiceBackend.Interfaces
{
public interface IUserRepository
{
    Task<ApplicationUser> FindByUsernameAsync(string username);
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}

}