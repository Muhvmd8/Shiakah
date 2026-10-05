using Shared.DTOs;
using Shared.DTOs.OutfitResults;

namespace ServiceAbstraction
{
    public interface IOutfitResultService
    {
        Task<Result<OutfitResultDto>> GenerateAsync(string userId, GenerateOutfitRequest request);
        Task<Result<IEnumerable<OutfitResultDto>>> GetAllAsync();
        Task<Result<IEnumerable<OutfitResultDto>>> GetAllForUserAsync(string userId);
    }
}