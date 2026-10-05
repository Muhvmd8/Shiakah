namespace Domain.Models
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string IconUrl { get; set; } = default!;
    }
}
