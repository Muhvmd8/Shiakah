using Shared.DTOs;
using Shared.DTOs.Wardrobe;

namespace ServiceAbstraction
{
    public interface IWardrobeService
    {
        Task<Result<int>> AddItemAsync(string userId, CreateWardrobeItemWithAttributesRequest request);
        Task<Result<int>> AddItemToMyClosetAsync(string userId, CreateWardrobeItemRequest request);
        Task<Result<WardrobeDto>> GetMyClosetAsync(string userId);
    }
}