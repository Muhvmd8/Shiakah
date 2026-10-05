using Shared.DTOs;
using Shared.DTOs.Category;

namespace ServiceAbstraction
{
    public interface ICategoryService
    {
        Task<Result<int>> CreateCategoryAsync(
            CreateCategoryDto createCategoryDto);

        Task<Result<int>> UpdateCategoryAsync(
            UpdateCategoryDto updateCategoryDto);

        Task<Result> DeleteCategoryAsync(int id);

        Task<Result<CategoryDto>> GetCategoryByIdAsync(int id);

        Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();

        Task<Result<IEnumerable<CategoryDetailedDto>>>
            GetAllCategoriesForAdminAsync();
    }
}
