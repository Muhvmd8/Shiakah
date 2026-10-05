using Microsoft.AspNetCore.Http;
using Shared.DTOs.Lookup;

namespace Shared.DTOs.Category
{
    public class UpdateCategoryDto : UpdateLookupDto
    {
        public IFormFile? Icon { get; set; }
    }
}
