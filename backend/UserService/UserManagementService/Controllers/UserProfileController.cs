using Microsoft.AspNetCore.Mvc;
using UserManagementService.Interfaces;
using UserManagementService.Models;
using UserManagementService.DTOs;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace UserManagementService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedUsers = await _userService.GetAllUsersAsync(page, pageSize);

            // Sort users by FollowersCount
            var sortedUsers = paginatedUsers.Items.OrderByDescending(user => user.FollowersCount);

            // HATEOAS Links
            var response = new
            {
            Data = sortedUsers.Select(user => new
            {
                user.UserId,
                user.Username,
                user.Bio,
                user.FollowersCount,
                user.FollowingCount,
                user.ImageUrl,
                user.Name,
                _links = new
                {
                self = Url.Action(nameof(GetUserById), new { id = user.UserId }),
                follow = Url.Action(nameof(FollowUser), new { userId = user.UserId, targetId = user.UserId }),
                unfollow = Url.Action(nameof(UnfollowUser), new { userId = user.UserId, targetId = user.UserId })
                }
            }),
            Pagination = new
            {
                paginatedUsers.TotalItems,
                paginatedUsers.CurrentPage,
                paginatedUsers.PageSize,
                paginatedUsers.TotalPages,
                _links = new
                {
                next = page < paginatedUsers.TotalPages ? Url.Action(nameof(GetAllUsers), new { page = page + 1, pageSize }) : null,
                previous = page > 1 ? Url.Action(nameof(GetAllUsers), new { page = page - 1, pageSize }) : null
                }
            }
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var response = new
            {
                user.UserId,
                user.ImageUrl,
                user.Name,
                user.Username,
                user.Bio,
                user.Skills,
                user.TopicsOfInterest,
                user.Followers,
                user.Following,
                user.FollowersCount,
                user.FollowingCount,
                _links = new
                {
                    self = Url.Action(nameof(GetUserById), new { id = user.UserId }),
                    update = Url.Action(nameof(UpdateUser), new { id = user.UserId }),
                    delete = Url.Action(nameof(DeleteUser), new { id = user.UserId })
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserProfileCreateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = _mapper.Map<User>(user);
            await _userService.CreateUserAsync(newUser);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserId }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserProfileUpdateDto updatedUser)
        {
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdToken != id)
            {
                return Unauthorized(new { Message = "You are not authorized to update this user." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(id, updatedUser);
            if (!result)
            {
                return NotFound(new { Message = "User not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdToken != id)
            {
                return Unauthorized(new { Message = "You are not authorized to delete this user." });
            }

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "User not found." });
            }

            return NoContent();
        }

        [HttpPost("{userId}/follow/{targetId}")]
        public async Task<IActionResult> FollowUser(string userId, string targetId)
        {
            if (userId == targetId)
            {
                return BadRequest(new { Message = "You cannot follow yourself." });
            }

            var result = await _userService.FollowUserAsync(userId, targetId);
            if (!result)
            {
                return NotFound(new { Message = "Target user not found." });
            }

            return NoContent();
        }

        [HttpPost("{userId}/unfollow/{targetId}")]
        public async Task<IActionResult> UnfollowUser(string userId, string targetId)
        {
            if (userId == targetId)
            {
                return BadRequest(new { Message = "You cannot unfollow yourself." });
            }

            var result = await _userService.UnfollowUserAsync(userId, targetId);
            if (!result)
            {
                return NotFound(new { Message = "Target user not found." });
            }

            return NoContent();
        }

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(string userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var followers = await _userService.GetFollowersAsync(userId, page, pageSize);
            var response = new
            {
                Data = followers.Items,
                Pagination = new
                {
                    followers.TotalItems,
                    followers.CurrentPage,
                    followers.PageSize,
                    followers.TotalPages,
                    _links = new
                    {
                        next = page < followers.TotalPages ? Url.Action(nameof(GetFollowers), new { userId, page = page + 1, pageSize }) : null,
                        previous = page > 1 ? Url.Action(nameof(GetFollowers), new { userId, page = page - 1, pageSize }) : null
                    }
                }
            };

            return Ok(response);
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing(string userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var following = await _userService.GetFollowingAsync(userId, page, pageSize);

            var response = new
            {
                Data = following.Items,
                Pagination = new
                {
                    following.TotalItems,
                    following.CurrentPage,
                    following.PageSize,
                    following.TotalPages,
                    _links = new
                    {
                        next = page < following.TotalPages ? Url.Action(nameof(GetFollowing), new { userId, page = page + 1, pageSize }) : null,
                        previous = page > 1 ? Url.Action(nameof(GetFollowing), new { userId, page = page - 1, pageSize }) : null
                    }
                }
            };

            return Ok(response);
        }
    }
}
