using Grpc.Core;
using UserManagementService.Grpc;
using UserManagementService.Interfaces;
namespace UserManagementService.Services
{
    public class GrpcUserService : Grpc.UserService.UserServiceBase
    {
        private readonly IUserRepository _userRepository;

        public GrpcUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public override async Task<UserResponse> GetUserById(GetUserRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUser(request.UserId);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            return await Task.FromResult(new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Age = user.Age,
                Bio = user.Bio,
                Followers = { user.Followers },
                Following = { user.Following },
                ImageUrl = user.ImageUrl
            });
        }
    } 
}
