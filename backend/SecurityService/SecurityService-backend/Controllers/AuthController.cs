using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecurityServiceBackend.DTOs;
using SecurityServiceBackend.Interfaces;

namespace SecurityServiceBackend.Contrlollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(registerDTO);
                if (result == null) return BadRequest("Failed to register user.");
                return Ok("User registered successfully.");
            }
            return BadRequest("Invalid input.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(loginDTO);
                if (result == null) return BadRequest("Invalid credentials.");
                return Ok(result);
            }
            return BadRequest("Invalid input.");
        }
    }

}