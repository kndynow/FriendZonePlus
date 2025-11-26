using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FriendZonePlus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var userId = await _userService.RegisterUserAsync(dto);

                return Ok(new { Id = userId, Message = "User Successfully created!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An internal error occurred" });
            }
        }
    }
}
