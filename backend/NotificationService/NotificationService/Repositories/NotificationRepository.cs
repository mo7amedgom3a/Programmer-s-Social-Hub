using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using NotificationService.Interfaces;

namespace NotificationService.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.RecipientUserId == userId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

    }
}
