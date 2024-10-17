namespace PostService.AsyncDataService.Models
{
    public class PostCommentedEvent
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public string SenderUserId { get; set; }
        public string RecipientUserId { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentedAt { get; set; }
    }
}
