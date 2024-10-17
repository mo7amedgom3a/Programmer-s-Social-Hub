using Microsoft.AspNetCore.SignalR;
using NotificationService.Interfaces;
using NotificationService.Models;
using NotificationService.Services;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task MarkNotificationAsReadAsync(string notificationId)
        {
            Guid notificationIdGuid = Guid.Parse(notificationId);
            await _notificationService.MarkNotificationAsReadAsync(notificationIdGuid);
            await Clients.All.SendAsync("NotificationRead", notificationId);
        }
        public async Task DeleteNotificationAsync(string notificationId)
        {
            Guid notificationIdGuid = Guid.Parse(notificationId);
            await _notificationService.DeleteNotificationAsync(notificationIdGuid);
            await Clients.All.SendAsync("NotificationDeleted", notificationId);
        }
        public async Task CreateNotificationAsync(Notification notification)
        {
            await _notificationService.CreateNotificationAsync(notification);
            await Clients.All.SendAsync("NotificationCreated", notification);
        }
        public async Task GetNotificationAsync(string notificationId)
        {
            Guid notificationIdGuid = Guid.Parse(notificationId);
            var notification = await _notificationService.GetNotificationAsync(notificationIdGuid);
            await Clients.Caller.SendAsync("NotificationReceived", notification);
        }
        public async Task GetUserNotificationsAsync(string userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            await Clients.Caller.SendAsync("NotificationsReceived", notifications);
        }
    }
}
