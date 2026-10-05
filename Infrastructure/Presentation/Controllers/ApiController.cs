global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using System.Security.Claims;
using Shared.DTOs;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        [Authorize]
        protected string GetIdFromToken() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (!result.Succeeded)
                return BadRequest(ApiResponse<T>.Failure(result.Message));

            return Ok(ApiResponse<T>.SuccessResponse(result.Data!, result.Message));
        }

        protected IActionResult HandleResult(Result result)
        {
            if (!result.Succeeded)
                return BadRequest(ApiResponse<object>.Failure(result.Message));

            return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
        }

        // Paginated result
        protected IActionResult HandlePaginated<T>(PaginatedResultDto<T> result, string message = "Success")
        {
            return Ok(ApiResponse<PaginatedResultDto<T>>
                .SuccessResponse(result, message));
        }
    }
}