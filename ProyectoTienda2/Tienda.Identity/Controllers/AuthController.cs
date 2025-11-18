using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tienda.Identity.Data;
using Tienda.Identity.Dto;
using Tienda.Identity.Services;

namespace Tienda.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<IdentityUser> userManager,
            JwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(UserRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Error registrando usuario: {Error}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Error registrando usuario: " +
                             string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

            // Rol Member por defecto
            var roleAssignResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleAssignResult.Succeeded)
            {
                _logger.LogWarning("User registered but failed to assign Member role: {Error}",
                    string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)));
            }

            var token = await _jwtService.GenerateToken(user);

            _logger.LogInformation("Usuario registrado exitosamente: {Email}", request.Email);

            return Ok(new AuthResponse
            {
                Success = true,
                Token = token,
                Email = user.Email!,
                UserName = user.UserName!,
                Expiration = DateTime.Now.AddMinutes(60),
                Message = "Usuario registrado exitosamente"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogWarning("Intento de login fallido para: {Email}", request.Email);
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Email o contraseña incorrectos"
                });
            }

            var token = await _jwtService.GenerateToken(user);

            _logger.LogInformation("Login exitoso para: {Email}", request.Email);

            return Ok(new AuthResponse
            {
                Success = true,
                Token = token,
                Email = user.Email!,
                UserName = user.UserName!,
                Expiration = DateTime.Now.AddMinutes(60),
                Message = "Login exitoso"
            });
        }
    }
}