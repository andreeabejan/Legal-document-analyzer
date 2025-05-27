using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Simplex.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Domain.Entities.LoginRequest request) 
        {
            var result = await userService.LoginAsync(request.Email, request.Password);

            if (result.IsSuccess)
            {
                string token = "success";
                return Ok(token);
            }

            return Unauthorized(new { message = result.ErrorMessage });
          
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Domain.Entities.RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Name, email and password are required");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email
            };

            var hasher = new PasswordHasher<User>();
            user.HashedPassword = hasher.HashPassword(user, request.Password);

            var result = await userService.AddUserAsync(user);

            if (result.IsSuccess) 
            {
                return Ok(user);
            }

            return BadRequest(result.ErrorMessage); 
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await userService.GetUserByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await userService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                var result = await userService.AddUserAsync(user);
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetUserById), new { id = result.Data }, user);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                var fullError = ex.InnerException?.ToString() ?? ex.ToString();
                return BadRequest($"An error occurred while adding the user: {fullError}");

            }
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch");
            }
            var result = await userService.UpdateUserAsync(user);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await userService.DeleteUserAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

        //[HttpGet("conversations/{userId}")]
        //public async Task<IActionResult> GetUserConversationsHistory(Guid userId)
        //{
        //    var result = await userService.GetUserConversationsHistoryAsync(userId);
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result.Data);
        //    }
        //    return NotFound(result.ErrorMessage);
        //}
    }
}
