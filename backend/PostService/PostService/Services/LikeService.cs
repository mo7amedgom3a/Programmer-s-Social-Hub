using AutoMapper;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;
using PostService.AsyncDataService;
using PostService.AsyncDataService.Models;
namespace PostService.Services
{
    public class LikeService : ILikeService
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepository, IMapper mapper, IMessageBusClient messageBusClient, IPostRepository postRepository)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _postRepository = postRepository;
            
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(string postId)
        {
            return await _likeRepository.GetLikesByPostIdAsync(postId);
        }

        public async Task LikePostAsync(string postId, string userId)
        {
            await _likeRepository.LikePostAsync(postId, userId);
            var post = await _postRepository.GetPostByIdAsync(postId);
            var postLikedMessage = new PostLikedEvent
            {
                PostId = postId,
                RecipientUserId = post.AuthorId,
                SenderUserId = userId,
                LikedAt = DateTime.UtcNow
            };
            _messageBusClient.PublishEvent(postLikedMessage, "PostLikedEvent");
        }

        public async Task DislikePostAsync(string postId, string userId)
        {
            await _likeRepository.DislikePostAsync(postId, userId);
        }
    }
}