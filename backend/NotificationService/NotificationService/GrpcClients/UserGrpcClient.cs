using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using NotificationService.Grpc;
namespace NotificationService.GrpcClients
{
    public class GrpcUserClientService
{
    private readonly UserService.UserServiceClient _client;

    #region
//     service UserService {
//     rpc GetUserById (GetUserRequest) returns (UserResponse);
// }

// message GetUserRequest {
//     string userId = 1;
// }

// message UserResponse {
//     string userId = 1;
//     string username = 2;
//     string name = 3;
//     string gender = 4;
//     string bio = 5;
//     int32 age = 6;
//     repeated string skills = 7;
//     repeated string topicsOfInterest = 8;
//     string imageUrl = 9;
//     repeated string followers = 10;
//     repeated string following = 11;
// }
    #endregion

    public GrpcUserClientService(UserService.UserServiceClient client)
    {
        _client = client;
    }
    public async Task<UserResponse> GetUserByIdAsync(string userId)
    {
        var request = new GetUserRequest { UserId = userId };
        
        return await _client.GetUserByIdAsync(request);
    }
}

}