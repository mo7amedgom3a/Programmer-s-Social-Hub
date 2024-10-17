using System;

namespace NotificationService.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string RecipientUserId { get; set; }
        public string SenderUserId { get; set; }  
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Type { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
