using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace UserManagementService.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))] // Auto generate ObjectId
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; } = ObjectId.GenerateNewId().ToString(); // Auto generate UserId

        [BsonElement("username")]
        [BsonRequired]
        public string Username { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("bio")]
        public string Bio { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("skills")]
        public List<string> Skills { get; set; }

        [BsonElement("topicsOfInterest")]
        public List<string> TopicsOfInterest { get; set; }

        [BsonElement("imageUrl")]
        // [RegularExpression(@"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)")]
        public string ImageUrl { get; set; }
        [BsonElement("followers")]
        public List<string> Followers { get; set; } = new List<string>();
        [BsonElement("following")]
        public List<string> Following { get; set; } = new List<string>();
    }
}
