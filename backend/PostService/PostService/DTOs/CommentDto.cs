using MongoDB.Bson.Serialization.Attributes;
using PostService.Models;

namespace PostService.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public UserMetadata userMetadata { get; set; }
        public string PostId { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public string CodeSection { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
