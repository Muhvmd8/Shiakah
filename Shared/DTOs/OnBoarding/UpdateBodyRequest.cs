using Shared.Enums;

namespace Shared.DTOs.OnBoarding
{
    public class UpdateBodyRequest
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public BodyType BodyType { get; set; }
    }
}
