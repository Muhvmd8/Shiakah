using Shared.Enums;

namespace Shared.DTOs.OnBoarding
{
    public class UpdatePreferencesRequest
    {
        public List<Style> Styles { get; set; } = [];

        public List<PreferredColor> PreferredColors { get; set; } = [];
    }
}
