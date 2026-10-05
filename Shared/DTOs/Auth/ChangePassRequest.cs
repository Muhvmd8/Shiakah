namespace Shared.DTOs.Auth
{
    public class ChangePassRequest
    {
        public string Email { get; set; } = default!;
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
