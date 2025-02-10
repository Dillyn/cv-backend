using FirstAPI.Models;
using FirstAPI.Services;
using FirstAPI.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("/user")] //set route
    [ApiController] // Bring in apicontroller
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    return NoContent();
                }
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "Invalid user data" });
            }
            try
            {
                _userService.CreateUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]  User user)
        {
            if (user == null || id != user.Id)
            {
                return BadRequest(new { message = "Invalid user data or ID mismatch" });
            }
            try
            {
                await _userService.UpdateUser(user);
                return Ok(new { message = "User updated successfully" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Business Logic

        [HttpGet("by-first-letter/{letter}")]
        public IActionResult GetUsersByFirstLetter(char letter)
        {
            try
            {
                var users = _userService.FindUsersByFirstLetter(letter); // Call service method 
                if (users == null || users.Count == 0)
                {
                    return Ok(users);
                }
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                // You can catch a specific exception if it's custom, or handle all exceptions like this
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
