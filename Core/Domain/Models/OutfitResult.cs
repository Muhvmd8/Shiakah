using Domain.Models.User;

namespace Domain.Models
{
    public class OutfitResult : BaseEntity<int>
    {
        private OutfitResult()
        {
        }

        public OutfitResult(
            string applicationUserId,
            string top,
            string bottom,
            string footwear,
            string accessories,
            string recommendedImageUrl,
            string searchQuery,
            string stylingTips,
            string summary)
        {
            ApplicationUserId = applicationUserId;
            Top = top;
            Bottom = bottom;
            Footwear = footwear;
            Accessories = accessories;
            RecommendedImageUrl = recommendedImageUrl;
            SearchQuery = searchQuery;
            StylingTips = stylingTips;
            Summary = summary;
            CreatedAt = DateTime.UtcNow;
        }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string Top { get; set; } = null!;
        public string Bottom { get; set; } = null!;
        public string Footwear { get; set; } = null!;
        public string Accessories { get; set; } = null!;
        public string RecommendedImageUrl { get; set; } = null!;
        public string SearchQuery { get; set; } = null!;
        public string StylingTips { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
