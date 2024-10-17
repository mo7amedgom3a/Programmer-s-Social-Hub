using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PostService.Models
{
    public class Like
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public UserMetadata UserMetadata { get; set; }
        [BsonElement("PostId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
