using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.OutfitResults;

namespace Presentation.Controllers
{
    public class OutfitResultController(IOutfitResultService outfitResultService)
        : ApiController
    {
        [Authorize(Roles = "User")]
        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<OutfitResultDto>>> Generate(
            [FromForm] GenerateOutfitRequest request)
        {
            var userId = GetIdFromToken();
            var result = await outfitResultService.GenerateAsync(userId, request);
            return ApiResponse<OutfitResultDto>.SuccessResponse(result.Data!, result.Message);
        }

        //[Authorize]
        //[HttpGet("all-outfits")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<OutfitResultDto>>>> GetAll()
        //{
        //    var result = await outfitResultService.GetAllAsync();
        //    return ApiResponse<IEnumerable<OutfitResultDto>>.SuccessResponse(result.Data!, result.Message);
        //}

        [Authorize]
        [HttpGet("my-looks")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OutfitResultDto>>>> GetMyOutfits()
        {
            var userId = GetIdFromToken();
            var result = await outfitResultService.GetAllForUserAsync(userId);
            return ApiResponse<IEnumerable<OutfitResultDto>>.SuccessResponse(result.Data!, result.Message);
        }
    }
}
