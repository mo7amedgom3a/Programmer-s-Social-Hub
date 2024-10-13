using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SecurityServiceBackend.DTOs;
using SecurityServiceBackend.Interfaces;
using SecurityServiceBackend.Models;
using SecurityService.AsyncDataServices;
using SecurityService.DTOs;

namespace SecurityServiceBackend.Services
{}
    public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMessageBusClient _messageBusClient;

    public AuthService(IUserRepository userRepository, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IMessageBusClient messageBusClient)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _messageBusClient = messageBusClient;
    }
    public async Task<ApplicationUser> RegisterAsync(RegisterDTO registerUserDto)
    {
        var user = _mapper.Map<ApplicationUser>(registerUserDto);

        var result = await _userRepository.CreateUserAsync(user, registerUserDto.Password);
        if (result != null)
        {
            Console.WriteLine("User registered successfully.");
            // Publish message to RabbitMQ
            var userDto = _mapper.Map<UserRegistrationDto>(user);
            userDto.UserId = user.Id;
            userDto.Event = "UserRegistered";
            _messageBusClient.PublishNewUser(userDto);
            return user;
        }
        return null;
            
    }

    public async Task<string> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userRepository.FindByUsernameAsync(loginDTO.Username);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, loginDTO.Password))
        {
            return null; // Login failed
        }

        // Generate JWT token
        return _jwtTokenGenerator.GenerateToken(user);
    }
}