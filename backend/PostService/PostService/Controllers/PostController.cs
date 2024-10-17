using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.DTOs;
using PostService.Interfaces;

namespace PostService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeRepository;
        private readonly ISavedPostService _savedPostService;

        public PostController(IPostService postService, ILikeService likeRepository, ISavedPostService savedPostService)
        {
            _postService = postService;
            _likeRepository = likeRepository;
            _savedPostService = savedPostService;
        }

      [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedPosts = await _postService.GetPaginatedPostsAsync(page, pageSize);
            return Ok(paginatedPosts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                return Ok(post);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("user/posts/{authorId}")]
        public async Task<IActionResult> GetPostsByUserId(string authorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var post = await _postService.GetPaginatedPostsByIdAsync(authorId, page, pageSize);
                return Ok(post);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        private bool MatchId(string id, string userId)
        {
            return id == userId;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var valid = MatchId(userIdToken, postDto.AuthorId);
            if (!valid) return Unauthorized("You are not authorized to create this post");
           
            await _postService.CreatePostAsync(postDto);
            var post = await _postService.GetPostsByUserIdAsync(postDto.AuthorId);
            try {
                return Created("api/post", post.Last());
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePostDto postDto)
        {
            try
            {
                var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var valid = MatchId(userIdToken, postDto.AuthorId);
                if (!valid) return Unauthorized("You are not authorized to update this post");
                await _postService.UpdatePostAsync(id, postDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await _postService.GetPostByIdAsync(id);
            Console.WriteLine("user id --> " + userIdToken);
            var valid = MatchId(userIdToken, post.AuthorId);
            if (!valid) return Unauthorized("You are not authorized to delete this post");
            await _postService.DeletePostAsync(id);
            return NoContent();
        }
        [HttpGet("likes/{postId}")]
        public async Task<IActionResult> GetLikesByPostId(string postId)
        {
            var likes = await _likeRepository.GetLikesByPostIdAsync(postId);
            return Ok(likes);
        }
        [HttpPost("likes/{postId}/{userId}")]
        public async Task<IActionResult> LikePost(string postId, string userId)
        {
            await _likeRepository.LikePostAsync(postId, userId);
            return NoContent();
        }
        [HttpDelete("likes/{postId}/{userId}")]
        public async Task<IActionResult> DislikePost(string postId, string userId)
        {
            await _likeRepository.DislikePostAsync(postId, userId);
            return NoContent();
        }
        [HttpGet("saved/{userId}")]
        public async Task<IActionResult> GetSavedPostsByUserId(string userId)
        {
            var savedPosts = await _savedPostService.GetSavedPostsByUserIdAsync(userId);
            return Ok(savedPosts);
        }
        [HttpPost("saved/{postId}/{userId}")]
        public async Task<IActionResult> SavePost(string postId, string userId)
        {
            await _savedPostService.SavePostAsync(postId, userId);
            return NoContent();
        }
        [HttpDelete("saved/{postId}/{userId}")]
        public async Task<IActionResult> UnsavePost(string postId, string userId)
        {
            await _savedPostService.UnsavePostAsync(postId, userId);
            return NoContent();
        }
    }
}
