using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthUserContext _context;

        public UserController(AuthUserContext context)
        {
            _context = context;
        }

        [HttpGet("getAllUsers")]

        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users != null )
            {
                return StatusCode(200, users);
            }

            return BadRequest("No users found");


        }



        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] NewUserDto newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Name) || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest("Invalid user data.");
            }
            var user = new User
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Password = newUser.Password

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return StatusCode(201, user);
        }

        [HttpPut("updateUser/{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] NewUserDto updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (updatedUser == null ||
                string.IsNullOrEmpty(updatedUser.Name) ||
                string.IsNullOrEmpty(updatedUser.Email) ||
                string.IsNullOrEmpty(updatedUser.Password))
            {
                return BadRequest("Invalid user data.");
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            await _context.SaveChangesAsync();

            return StatusCode(200, user);
        }


        [HttpDelete("deleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return StatusCode(200, "User successfully deleted.");
        }
    }
}
