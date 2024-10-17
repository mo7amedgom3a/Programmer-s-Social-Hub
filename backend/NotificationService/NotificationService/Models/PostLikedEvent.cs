namespace NotificationService.Models
{
    public class PostLikedEvent
    {
        public string PostId { get; set; }
        public string SenderUserId { get; set; }
        public string RecipientUserId { get; set; }
        public DateTime LikedAt { get; set; }
    }
}
