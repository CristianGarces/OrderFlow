using System.Collections.Generic;

namespace Tienda.Identity.Dto
{
    public record UserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
    }

    public record UserResponse
    {
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
