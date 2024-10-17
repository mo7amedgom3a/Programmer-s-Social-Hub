using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Services;

namespace PostService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByPostIdAsync(postId);
                return Ok(comments);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> AddComment(string postId, [FromBody] CreateCommentDto commentDto)
        {
            try
            {
                await _commentService.AddCommentToPostAsync(postId, commentDto);
                return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = postId }, commentDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdateComment(string postId,  [FromBody] UpdateCommentDto commentDto)
        {
            try
            {
                await _commentService.UpdateCommentAsync(postId, commentDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{postId}/{commentId}")]
        public async Task<IActionResult> DeleteComment(string postId, string commentId)
        {
            try
            {
                await _commentService.DeleteCommentAsync(postId, commentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}