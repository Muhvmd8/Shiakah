using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Wardrobe
{
    public class CreateWardrobeItemRequest
    {
        public IFormFile Image { get; set; } = null!;
    }
}