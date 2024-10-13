using SecurityServiceBackend.Models;

namespace SecurityServiceBackend.Interfaces
{
    public interface IUserManager
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}