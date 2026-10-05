namespace Shared.DTOs.Wardrobe
{
    public class WardrobeItemDto
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public ItemAttributeDto? Attributes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
