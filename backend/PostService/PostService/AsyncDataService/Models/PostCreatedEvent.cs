namespace PostService.AsyncDataService.Models
{
    public class PostCreatedEvent
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}