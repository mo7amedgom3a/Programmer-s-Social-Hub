using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotificationService.Models;
using NotificationService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.GrpcClients;
using System.Text;
using NotificationService.Grpc;
using NotificationService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventProcessor> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventProcessor(IServiceProvider serviceProvider, ILogger<EventProcessor> logger, IHubContext<NotificationHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task ProcessEventAsync(string message)
        {
            var eventType = DetermineEventType(message);

            switch (eventType)
            {
                case EventType.PostLiked:
                    await HandlePostLikedEventAsync(message);
                    break;
                case EventType.PostCommented:
                    await HandlePostCommentedEventAsync(message);
                    break;
                default:
                    _logger.LogWarning("Event type not recognized.");
                    break;
            }
        }

        private EventType DetermineEventType(string notificationMessage)
        {
            try
            {
                var eventTypeWrapper = JsonConvert.DeserializeObject<dynamic>(notificationMessage);
                string eventType = eventTypeWrapper?.EventType;

                if (eventType == "PostLikedEvent")
                {
                    return EventType.PostLiked;
                }
                else if (eventType == "PostCommentedEvent")
                {
                    return EventType.PostCommented;
                }
                else
                {
                    return EventType.Undetermined;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error determining event type: {ex.Message}");
                return EventType.Undetermined;
            }
        }

        private async Task HandlePostLikedEventAsync(string message)
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userGrpcClient = scope.ServiceProvider.GetRequiredService<GrpcUserClientService>();

            var eventData = JsonConvert.DeserializeObject<dynamic>(message).Data;
            var postLikedEvent = JsonConvert.DeserializeObject<PostLikedEvent>(eventData.ToString());

            // Fetch user details using gRPC
            UserResponse senderDetails = await userGrpcClient.GetUserByIdAsync(postLikedEvent.SenderUserId);
            string name = senderDetails.Username;
            var notification = new Notification
            {
            RecipientUserId = postLikedEvent.RecipientUserId,
            SenderUserId = postLikedEvent.SenderUserId,
            Message = $"{name} liked your post {postLikedEvent.PostId}",
            Type = "Like",
            Timestamp = postLikedEvent.LikedAt,
            IsRead = false
            };

            await notificationService.CreateNotificationAsync(notification);
            _logger.LogInformation("Notification for PostLikedEvent saved successfully.");
        }

        private async Task HandlePostCommentedEventAsync(string message)
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userGrpcClient = scope.ServiceProvider.GetRequiredService<GrpcUserClientService>();
            
            var eventData = JsonConvert.DeserializeObject<dynamic>(message).Data;
            var postCommentedEvent = JsonConvert.DeserializeObject<PostCommentedEvent>(eventData.ToString());

            // Fetch user details using gRPC
            UserResponse senderDetails = await userGrpcClient.GetUserByIdAsync(postCommentedEvent.SenderUserId);
            string name = senderDetails.Username;
            var notification = new Notification
            {
                RecipientUserId = postCommentedEvent.RecipientUserId,
                SenderUserId = postCommentedEvent.SenderUserId,
                Message = $"{name} commented on your post {postCommentedEvent.PostId}",
                Type = "Comment",
                Timestamp = postCommentedEvent.CommentedAt,
                IsRead = false
            };


                await notificationService.CreateNotificationAsync(notification);
                _logger.LogInformation("Notification for PostCommentedEvent saved successfully.");

        }

        public enum EventType
        {
            PostLiked,
            PostCommented,
            Undetermined
        }
    }
}
