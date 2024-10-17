// Repositories/CommentRepository.cs
using MongoDB.Driver;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
namespace PostService.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly IMongoCollection<Post> _postsCollection;
        private readonly IMapper _mapper;
        private readonly GrpcUserClientService _grpcUserClientService;

        public CommentRepository(MongoDBContext database, IMapper mapper, GrpcUserClientService grpcUserClientService)
        {
            _commentsCollection = database.Comments;
            _postsCollection = database.Posts;
            _mapper = mapper;
            _grpcUserClientService = grpcUserClientService;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId)
        {
            var post = await _postsCollection.Find(p => p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
            {
               return null;
            }
            var comments = await _commentsCollection.Find(c => c.PostId == postId)
                                                    .SortByDescending(c => c.CreatedAt)
                                                    .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
            var comment = await _commentsCollection.Find(c => c.Id == commentId).FirstOrDefaultAsync();
            return _mapper.Map<CommentDto>(comment);
        }
        public async Task AddCommentAsync(string postId, CreateCommentDto comment)
        { 
            var userMetadata = await _grpcUserClientService.GetUserByIdAsync(comment.AuthorId);
            if (userMetadata == null)
            {
                throw new Exception("User not found");
            }
            var newComment = _mapper.Map<Comment>(comment);
            newComment.PostId = postId;
            newComment.CreatedAt = DateTime.UtcNow;
            newComment.UpdatedAt = DateTime.UtcNow;
            newComment.UserMetadata = new UserMetadata
            {
                UserId = userMetadata.UserId,
                Username = userMetadata.Username,
                Name = userMetadata.Name,
                Bio = userMetadata.Bio,
                ImageUrl = userMetadata.ImageUrl
            };
            await _commentsCollection.InsertOneAsync(newComment);

            // Add the comment's ID to the Post's CommentIds list
            var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
            var update = Builders<Post>.Update.Push(p => p.CommentIds, newComment.Id);
            await _postsCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateCommentAsync(string postId, Comment comment)
        {
            await _commentsCollection.ReplaceOneAsync(c => c.Id == comment.Id, comment);
        }

        public async Task DeleteCommentAsync(string postId, string commentId)
        {
            // Delete the comment from the Comments collection
            var filter = Builders<Comment>.Filter.Eq(c => c.Id, commentId);
            await _commentsCollection.DeleteOneAsync(filter);

            // Remove the comment's ID from the Post's CommentIds list
            var postFilter = Builders<Post>.Filter.Eq(p => p.Id, postId);
            var update = Builders<Post>.Update.Pull(p => p.CommentIds, commentId);
            await _postsCollection.UpdateOneAsync(postFilter, update);
        }
    }
}
