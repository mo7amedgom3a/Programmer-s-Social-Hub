using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PostService.Models;

namespace PostService.Repositories
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("PostService");
        }

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
        public IMongoCollection<SavedPost> SavedPosts => _database.GetCollection<SavedPost>("SavedPosts");
        public IMongoCollection<Like> Likes => _database.GetCollection<Like>("Likes");

        // Add more collections as needed
    }
}
