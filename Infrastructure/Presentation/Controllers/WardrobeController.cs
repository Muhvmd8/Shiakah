using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Wardrobe;

namespace Presentation.Controllers
{
    public class WardrobeController(IWardrobeService wardrobeService)
        : ApiController
    {
        [HttpPost("add-to-my-closet")]
        public async Task<ActionResult<ApiResponse<int>>> AddToMyCloset(
            [FromForm] CreateWardrobeItemRequest request)
        {
            var userId = GetIdFromToken();
            var result = await wardrobeService.AddItemToMyClosetAsync(userId, request);
            return ApiResponse<int>.SuccessResponse(result.Data, result.Message);
        }

        [HttpPost("add-item-with-attributes")]
        public async Task<ActionResult<ApiResponse<int>>> AddItemWithAttributes(
            [FromForm] CreateWardrobeItemWithAttributesRequest request)
        {
            var userId = GetIdFromToken();
            var result = await wardrobeService.AddItemAsync(userId, request);
            return ApiResponse<int>.SuccessResponse(result.Data, result.Message);
        }

        [HttpGet("my-closet")]
        public async Task<ActionResult<ApiResponse<WardrobeDto>>> GetMyCloset()
        {
            var userId = GetIdFromToken();
            var result = await wardrobeService.GetMyClosetAsync(userId);
            return ApiResponse<WardrobeDto>.SuccessResponse(result.Data!, result.Message);
        }
    }
}
