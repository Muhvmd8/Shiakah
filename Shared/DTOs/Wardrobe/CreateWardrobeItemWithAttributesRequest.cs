using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Wardrobe
{
    public class CreateWardrobeItemWithAttributesRequest
    {
        public IFormFile Image { get; set; } = null!;
        public CreateItemAttributeRequest Attributes { get; set; } = null!;
    }
}