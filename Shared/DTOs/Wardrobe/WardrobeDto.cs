namespace Shared.DTOs.Wardrobe
{
    public class WardrobeDto
    {
        public int Id { get; set; }

        public IEnumerable<WardrobeItemDto> Items { get; set; } = [];
    }
}
