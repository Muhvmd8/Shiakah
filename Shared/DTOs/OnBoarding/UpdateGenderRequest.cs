using Shared.Enums;

namespace Shared.DTOs.OnBoarding
{
    public class UpdateGenderRequest
    {
        public Gender Gender { get; set; } = Gender.Unknown;
    }
}
