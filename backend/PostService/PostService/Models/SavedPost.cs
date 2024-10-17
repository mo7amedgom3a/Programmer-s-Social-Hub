using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PostService.DTOs;

namespace PostService.Models
{
    public class SavedPost
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public PostDto postDto { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }  // Reference to the User who saved the post
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
