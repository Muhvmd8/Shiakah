using Microsoft.AspNetCore.Http;
using Shared.DTOs.Wardrobe;

namespace Shared.DTOs.OutfitResults
{
    public class GenerateOutfitRequest
    {
        public IFormFile Image { get; set; } = null!;
        public CreateItemAttributeRequest Attributes { get; set; } = null!;
    }
}
