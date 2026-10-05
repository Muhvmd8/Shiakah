using Shared.Enums;
namespace Shared.DTOs.OnBoarding
{
    public class UpdateAppearanceRequest
    {
        public AgeRange AgeRange { get; set; }

        public SkinTone SkinTone { get; set; }
    }
}