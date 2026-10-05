namespace Shared.DTOs.OutfitResults
{
    public class CreateOutfitResultRequest
    {
        public string Top { get; set; } = null!;

        public string Bottom { get; set; } = null!;

        public string Footwear { get; set; } = null!;

        public string Accessories { get; set; } = null!;

        public string RecommendedImageUrl { get; set; } = null!;

        public string SearchQuery { get; set; } = null!;

        public string StylingTips { get; set; } = null!;

        public string Summary { get; set; } = null!;
    }
}
