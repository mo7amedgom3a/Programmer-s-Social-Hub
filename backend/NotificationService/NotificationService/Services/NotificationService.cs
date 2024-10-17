using NotificationService.Models;
using NotificationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<Notification> GetNotificationAsync(Guid notificationId)
        {
            return await _notificationRepository.GetNotificationByIdAsync(notificationId);
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateNotificationAsync(notification);
            }
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            await _notificationRepository.DeleteNotificationAsync(notificationId);
        }

    }
}
