using Microsoft.AspNetCore.Identity;
using SecurityServiceBackend.DTOs;
using SecurityServiceBackend.Models;

namespace SecurityServiceBackend.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
    }

}