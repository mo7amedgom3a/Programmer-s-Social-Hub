using MongoDB.Driver;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;
using AutoMapper;

namespace PostService.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly IMapper _mapper;
        private readonly GrpcUserClientService _grpcUserClientService;

        public PostRepository(MongoDBContext context, IMapper mapper, GrpcUserClientService grpcUserClientService)
        {
            _posts = context.Posts;
            _mapper = mapper;
            _grpcUserClientService = grpcUserClientService;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _posts.Find(post => true).ToListAsync();
        }
        public async Task<PaginatedPostsDto> GetPaginatedPostsAsync(int page, int pageSize)
        {
            var posts = await _posts.Find(post => true).Skip((page - 1) * pageSize).SortByDescending(post => post.CreatedAt).Limit(pageSize).ToListAsync();
            var totalPosts = await _posts.CountDocumentsAsync(post => true);
            return new PaginatedPostsDto
            {
                Posts = _mapper.Map<IEnumerable<PostDto>>(posts),
                TotalPosts = totalPosts,
                TotalPages = (int)Math.Ceiling(totalPosts / (double)pageSize),
                CurrentPage = page,
                PageSize = pageSize
            };
        }
        public async Task<Post> GetPostByIdAsync(string id)
        {
            return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
        }
        public async Task<PaginatedPostsDto> GetPaginatedPostsByIdAsync(string userId, int page, int pageSize)
        {
            var posts = await _posts.Find(post => post.userMetadata.UserId == userId).Skip((page - 1) * pageSize).SortByDescending(post => post.CreatedAt).Limit(pageSize).ToListAsync();
            var totalPosts = await _posts.CountDocumentsAsync(post => post.userMetadata.UserId == userId);
            return new PaginatedPostsDto
            {
                Posts = _mapper.Map<IEnumerable<PostDto>>(posts),
                TotalPosts = totalPosts,
                TotalPages = (int)Math.Ceiling(totalPosts / (double)pageSize),
                CurrentPage = page,
                PageSize = pageSize
            };
        }
        private string EscapeStrings(string input)
        {
            // replace "" -> '' and \n -> \\n and \t -> \\t ...
            return input.Replace("\"", "\'").Replace("\n", "\\n").Replace("\t", "\\t")
                .Replace("\r", "\\r").Replace("\f", "\\f").Replace("\b", "\\b")
                .Replace("\a", "\\a").Replace("\v", "\\v");
        }
        public async Task CreatePostAsync(Post post)
        {
            var userMetadata = await _grpcUserClientService.GetUserByIdAsync(post.AuthorId);

            post.userMetadata = new UserMetadata
            {
                UserId = userMetadata.UserId,
                Username = userMetadata.Username,
                Name = userMetadata.Name,
                Bio = userMetadata.Bio,
                ImageUrl = userMetadata.ImageUrl
            };
            // post.Code = EscapeStrings(post.Code);
            await _posts.InsertOneAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _posts.ReplaceOneAsync(p => p.Id == post.Id, post);
        }
        public async Task UpdatePostAsyncById(string id, UpdateDefinition<Post> update)
        {
            await _posts.UpdateOneAsync(p => p.Id == id, update);
        }
        public async Task DeletePostAsync(string id)
        {
            await _posts.DeleteOneAsync(post => post.Id == id);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId)
        {
            var posts = await _posts.Find(post => post.userMetadata.UserId == userId).ToListAsync();
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}
