namespace Tienda.Identity.Dto
{
    public record AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
    }
}