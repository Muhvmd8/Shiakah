using Microsoft.AspNetCore.Http;
using Shared.DTOs.Lookup;

namespace Shared.DTOs.Category
{
    public class CreateCategoryDto : CreateLookupDto
    {
        public IFormFile Icon { get; set; } = default!;
    }
}
