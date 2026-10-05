namespace Shared.DTOs.Auth
{
    public class AuthResult
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public List<string> UserRoles { get; set; } = [];
    }
}
