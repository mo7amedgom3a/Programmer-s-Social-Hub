using MongoDB.Driver;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;
using AutoMapper;

namespace PostService.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly IMongoCollection<Like> _likeCollection;
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMapper _mapper;
        private readonly GrpcUserClientService _grpcUserClientService;

        public LikeRepository(MongoDBContext mongoDBContext, IMapper mapper, GrpcUserClientService grpcUserClientService)
        {
            _likeCollection = mongoDBContext.Likes;
            _postCollection = mongoDBContext.Posts;
            _grpcUserClientService = grpcUserClientService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(string postId)
        {
            var likes = await _likeCollection.Find(like => like.PostId == postId).ToListAsync();
            return likes;
        }

        public async Task LikePostAsync(string postId, string userId)
        {
            var userMetadata = await _grpcUserClientService.GetUserByIdAsync(userId);
            if (userMetadata == null)
            {
                throw new Exception("User not found");
            }
            var like = new Like
            {
                PostId = postId,
                UserMetadata = new UserMetadata
                {
                    UserId = userMetadata.UserId,
                    Username = userMetadata.Username,
                    Name = userMetadata.Name,
                    Bio = userMetadata.Bio,
                    ImageUrl = userMetadata.ImageUrl
                }
            };
            await _likeCollection.InsertOneAsync(like);
            // Increment the likes count of the post
            await _postCollection.UpdateOneAsync(post => post.Id == postId, Builders<Post>.Update.Inc(post => post.Likes, 1));
        }

        public async Task DislikePostAsync(string postId, string userId)
        {
            var likePost = await _likeCollection.Find(like => like.PostId == postId && like.UserMetadata.UserId == userId).FirstOrDefaultAsync();
            if (likePost == null)
            {
                return;
            }
            await _likeCollection.DeleteOneAsync(like => like.PostId == postId && like.UserMetadata.UserId == userId);
            // Decrement the likes count of the post
            await _postCollection.UpdateOneAsync(post => post.Id == postId, Builders<Post>.Update.Inc(post => post.Likes, -1));
        }
    }
}