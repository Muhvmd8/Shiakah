namespace Shared.DTOs.Auth
{
    public class GenerateTokenResult
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
