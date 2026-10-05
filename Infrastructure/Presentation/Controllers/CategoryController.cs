namespace Presentation.Controllers;

//public class CategoryController(
//    ICategoryService categoryService)
//    : ApiController
//{
//    [Authorize(Roles = "Admin")]
//    [HttpPost]
//    public async Task<ActionResult<ApiResponse<int>>> CreateCategoryAsync(
//        [FromForm] CreateCategoryDto createCategoryDto)
//    {
//        var result = await categoryService.CreateCategoryAsync(
//            createCategoryDto);

//        return Ok(HandleResult(result));
//    }

//    [Authorize(Roles = "Admin")]
//    [HttpPut]
//    public async Task<ActionResult<ApiResponse<int>>> UpdateCategoryAsync(
//        [FromForm] UpdateCategoryDto updateCategoryDto)
//    {
//        var result = await categoryService.UpdateCategoryAsync(
//            updateCategoryDto);

//        return Ok(HandleResult(result));
//    }

//    [HttpGet]
//    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllCategoriesAsync()
//    {
//        var result = await categoryService.GetAllCategoriesAsync();

//        return Ok(HandleResult(result));
//    }

//    [Authorize(Roles = "Admin")]
//    [HttpGet("for-admin")]
//    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDetailedDto>>>> GetAllCategoriesForAdminAsync()
//    {
//        var result = await categoryService.GetAllCategoriesForAdminAsync();

//        return Ok(HandleResult(result));
//    }

//    [Authorize(Roles = "Admin")]
//    [HttpGet("{id}")]
//    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryByIdAsync(
//        int id)
//    {
//        var result = await categoryService.GetCategoryByIdAsync(id);

//        return Ok(HandleResult(result));
//    }

//    [Authorize(Roles = "Admin")]
//    [HttpDelete("{id}")]
//    public async Task<ActionResult<ApiResponse<object>>> DeleteCategoryAsync(
//        int id)
//    {
//        var result = await categoryService.DeleteCategoryAsync(id);

//        return Ok(HandleResult(result));
//    }
//}