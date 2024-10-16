using System;
using System.Text.Json;
using AutoMapper;
using UserManagementService.Interfaces;
using UserManagementService.Models;
using UserManagementService.Repositories;
using UserManagementService.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagementService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserCreated:
                    addUser(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<UserRegistrationDto>(notifcationMessage); // event = UserRegistered

            switch(eventType.Event)
            {
                case "UserRegistered":
                    Console.WriteLine("--> User Created Event Detected");
                    return EventType.UserCreated;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        private void addUser(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                Console.WriteLine($"--> User Created Event Processing: {message}");

                var userCreatedDto = JsonSerializer.Deserialize<UserRegistrationDto>(message);
                Console.WriteLine($"--> User Created Event Processing: {userCreatedDto.UserId}");

                try
                {
                    var user = _mapper.Map<User>(userCreatedDto);
                    userRepository.CreateUser(user);
                    Console.WriteLine("--> User Created");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add user to database: {ex.Message}");
                }
            }
        }

    }

    enum EventType
    {
        UserCreated,
        UserDeleted,
        Undetermined
        
    }
}