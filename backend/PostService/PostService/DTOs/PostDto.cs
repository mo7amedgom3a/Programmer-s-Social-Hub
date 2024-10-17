using MongoDB.Bson.Serialization.Attributes;
using PostService.Models;

namespace PostService.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }        
        public string AuthorId { get; set; }  // Reference to User Service
        public string Title { get; set; }
        public string Content { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public UserMetadata userMetadata { get; set; }
        public string Code { get; set; }
        [BsonIgnoreIfNull]
        public string Language { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Comments { get; set; } = new List<string>();
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
