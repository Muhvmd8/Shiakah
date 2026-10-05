using Domain.Models.User;

namespace Domain.Models
{
    public class Avatar : BaseEntity<int>
    {
        public string Url { get; set; } = default!;
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}