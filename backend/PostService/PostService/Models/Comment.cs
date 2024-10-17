// Models/Comment.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PostService.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Reference to the Post
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public UserMetadata UserMetadata { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string CodeSection { get; set; }
        [BsonIgnoreIfNull]
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
