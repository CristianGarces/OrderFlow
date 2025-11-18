using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tienda.Identity.Dto;

namespace Tienda.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ILogger<AdminController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserResponse>> CreateUser(AdminUserRequest request)
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
                return BadRequest(new UserResponse()
                {
                    Email = request.Email,
                    Message = "Error creating user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            if (!string.IsNullOrEmpty(request.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("User created but failed to assign role: {Error}",
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            _logger.LogInformation("Admin created user successfully: {Email}", request.Email);

            return Ok(new UserResponse
            {
                Email = request.Email,
                Message = "User created successfully"
            });
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<AdminUserResponse>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            if (users == null || !users.Any())
            {
                _logger.LogWarning("No users found");
                return Ok(new List<AdminUserResponse>());
            }

            var response = new List<AdminUserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(new AdminUserResponse
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }

            _logger.LogInformation("Admin retrieved {Count} users", users.Count);

            return Ok(response);
        }

        [HttpGet("users/{email}")]
        public async Task<ActionResult<AdminUserResponse>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found {Email}", email);
                return NotFound(new UserResponse()
                {
                    Email = email,
                    Message = "User not found",
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Admin retrieved user {Email}", email);
            return Ok(new AdminUserResponse()
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            });
        }

        [HttpPut("users/{email}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(string email, AdminUpdateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found {Email}", email);
                return NotFound(new UserResponse
                {
                    Email = email,
                    Message = "User not found"
                });
            }

            user.UserName = request.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Error updating user {Email}: {Error}", email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                return BadRequest(new UserResponse()
                {
                    Email = email,
                    Message = "Error updating user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            // Actualizar roles si se proporcionó
            if (!string.IsNullOrEmpty(request.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddToRoleAsync(user, request.Role);
                    if (!addResult.Succeeded)
                    {
                        _logger.LogWarning("User updated but failed to update role: {Error}",
                            string.Join(", ", addResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            _logger.LogInformation("Admin updated user successfully {Email}", email);

            return Ok(new UserResponse()
            {
                Email = email,
                Message = "User updated successfully"
            });
        }

        [HttpDelete("users/{email}")]
        public async Task<ActionResult<UserResponse>> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found {Email}", email);
                return NotFound(new UserResponse()
                {
                    Email = email,
                    Message = "User not found"
                });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Error deleting user {Email}: {Error}", email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(new UserResponse()
                {
                    Email = email,
                    Message = "Error deleting user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            _logger.LogInformation("Admin deleted user successfully {Email}", email);
            return Ok(new UserResponse()
            {
                Email = email,
                Message = "User deleted successfully"
            });
        }

       
    }
}