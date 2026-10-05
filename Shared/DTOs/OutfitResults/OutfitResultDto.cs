using System.Text.Json.Serialization;

namespace Shared.DTOs.OutfitResults
{
    public class OutfitAiResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("response")]
        public OutfitAiResultDto Response { get; set; } = null!;
    }
    public class OutfitAiResultDto
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = null!;

        [JsonPropertyName("items")]
        public OutfitItemsDto Items { get; set; } = null!;

        [JsonPropertyName("styling_tips")]
        public string StylingTips { get; set; } = null!;

        [JsonPropertyName("search_query")]
        public string SearchQuery { get; set; } = null!;

        [JsonPropertyName("recommended_image_url")]
        public string RecommendedImageUrl { get; set; } = null!;
    }

    public class OutfitItemsDto
    {
        [JsonPropertyName("top")]
        public string Top { get; set; } = null!;

        [JsonPropertyName("bottom")]
        public string Bottom { get; set; } = null!;

        [JsonPropertyName("footwear")]
        public string Footwear { get; set; } = null!;

        [JsonPropertyName("accessories")]
        public string Accessories { get; set; } = null!;
    }

    public class OutfitResultDto
    {
        public int Id { get; set; }
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
