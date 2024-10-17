using NotificationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification> GetNotificationAsync(Guid notificationId);
        Task CreateNotificationAsync(Notification notification);
        Task MarkNotificationAsReadAsync(Guid notificationId);
        Task DeleteNotificationAsync(Guid notificationId);
    }
}
