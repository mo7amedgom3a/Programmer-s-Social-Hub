namespace PostService.DTOs
{
    public class ReactionDto
    {
        public string UserId { get; set; }
        public string PostId { get; set; } // This is the ID of the post that the user is reacting to
        public bool IsLiked { get; set; }
    }
}
