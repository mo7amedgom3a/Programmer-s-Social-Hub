using SecurityService.DTOs;

namespace SecurityService.AsyncDataServices
{
public interface IMessageBusClient
{
    void PublishNewUser(UserRegistrationDto userDto);
}

}