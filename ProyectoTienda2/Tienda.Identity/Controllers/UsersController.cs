using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tienda.Identity.Dto;

namespace Tienda.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ILogger<UsersController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(UserRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Error creating user: {Error}", string.Join(", ", result.Errors.Select(e => e.Description)));

                // BadRequest: mostrar el código (404) si hay error
                return BadRequest(new UserResponse()
                {
                    Email = request.Email,
                    Message = "Error creating user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            _logger.LogInformation("User created succesfully: {Email}", request.Email);

            return Ok(new UserResponse
            {
                Email = request.Email,
                Message = "User created successfully"
            });
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found {Email}", email);
                return BadRequest(new UserResponse()
                {
                    Email = email,
                    Message = "User not found",
                });
            }

            _logger.LogInformation("User retrieved succesfully {Email}", email);
            return Ok(new UserResponse()
            {
                Email = email,
                Message = "User retrieved succesfully"
            });
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult<UserResponse>> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                _logger.LogWarning("User not found {Email}", email);
                return BadRequest(new UserResponse()
                {
                    Email = email,
                    Message = "User not found"
                });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Error deleting user {Email}", email);
                return BadRequest(new UserResponse()
                {
                    Email = email,
                    Message = "Error deleting user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            _logger.LogInformation("User deleted succesfully {Email}", email);
            return Ok(new UserResponse()
            {
                Email = email,
                Message = "User deleted succesfully"
            });
        }
    }
}
