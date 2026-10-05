using Domain.Models.User;

namespace Domain.Models
{
    public class Wardrobe : BaseEntity<int>
    {
        public string ApplicationUserId { get; private set; } = null!;
        public ApplicationUser User { get; private set; } = null!;
        public ICollection<WardrobeItem> Items { get; set; } = [];
        public Wardrobe(string applicationUserId)
        {
            ApplicationUserId = applicationUserId;
        }
    }
}