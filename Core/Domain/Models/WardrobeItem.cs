namespace Domain.Models
{
    public class WardrobeItem : BaseEntity<int>
    {
        public string ImageUrl { get; set; } = null!;
        public ItemAttribute? Attributes { get; set; }
        public Wardrobe Wardrobe { get; set; } = default!;
        public int WardrobeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public WardrobeItem() { }
        public WardrobeItem(string imageUrl, int wardrobeId, ItemAttribute? attributes = null, DateTime? updatedAt = null)
        {
            ImageUrl = imageUrl;
            Attributes = attributes;
            WardrobeId = wardrobeId;
            UpdatedAt = updatedAt;
        }
    }
}