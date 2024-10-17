using AutoMapper;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;
using PostService.AsyncDataService;
using PostService.AsyncDataService.Models;
namespace PostService.Services
{
    public class Postservice : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public Postservice(IPostRepository postRepository, IMapper mapper, ICommentRepository commentRepository, IMessageBusClient messageBusClient)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _commentRepository = commentRepository;
            _messageBusClient = messageBusClient;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<PaginatedPostsDto> GetPaginatedPostsAsync(int page, int pageSize)
        {
            var paginatedPosts = await _postRepository.GetPaginatedPostsAsync(page, pageSize);
            return _mapper.Map<PaginatedPostsDto>(paginatedPosts);
        }
        public async Task<PostDto> GetPostByIdAsync(string id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null) throw new KeyNotFoundException("Post not found");
            return _mapper.Map<PostDto>(post);
        }
        public async Task<PaginatedPostsDto> GetPaginatedPostsByIdAsync(string userId, int page, int pageSize)
        {
            var paginatedPosts = await _postRepository.GetPaginatedPostsByIdAsync(userId, page, pageSize);
            return _mapper.Map<PaginatedPostsDto>(paginatedPosts);
        }

        public async Task CreatePostAsync(CreatePostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            
            await _postRepository.CreatePostAsync(post);
            var postCreatedMessage = new PostCreatedEvent
            {
                PostId = post.Id,
                UserId = post.AuthorId,
                CreatedAt = post.CreatedAt,
                PostContent = post.Content
            };
            _messageBusClient.PublishEvent(postCreatedMessage, "PostCreatedEvent");
        }

        public async Task UpdatePostAsync(string id, UpdatePostDto postDto)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(id);
            if (existingPost == null) throw new KeyNotFoundException("Post not found");

            _mapper.Map(postDto, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdatePostAsync(existingPost);
        }

        public async Task DeletePostAsync(string id)
        {
            await _postRepository.DeletePostAsync(id);
        }
        public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

    }
}
