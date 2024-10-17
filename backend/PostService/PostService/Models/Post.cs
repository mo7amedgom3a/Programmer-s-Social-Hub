using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PostService.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AuthorId { get; set; }  
        public UserMetadata userMetadata { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        [BsonIgnoreIfNull]
        public string Code { get; set; }
        [BsonIgnoreIfNull]
        public string Language { get; set; }
        [BsonIgnoreIfNull]
        public List<string> Images { get; set; } = new List<string>();
        public List<string> CommentIds { get; set; } = new List<string>();
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }

public class UserMetadata
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public string ImageUrl { get; set; }
}
}
