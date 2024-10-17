using NotificationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId);
        Task<Notification> GetNotificationByIdAsync(Guid notificationId);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Guid notificationId);
    
    }
}
