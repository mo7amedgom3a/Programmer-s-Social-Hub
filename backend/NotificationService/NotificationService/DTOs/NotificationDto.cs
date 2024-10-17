using System;

namespace NotificationService.DTOs
{
    public class NotificationDto
    {
        public string RecipientUserId { get; set; }  // The user ID of the recipient
        public string SenderUserId { get; set; }  // The user ID of the sender
        public string Message { get; set; }  // The notification message content
        public DateTime Timestamp { get; set; }  // Timestamp for when the notification is created
    }
}
