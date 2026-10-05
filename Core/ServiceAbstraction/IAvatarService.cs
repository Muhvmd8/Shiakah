using Shared.DTOs.Avatar;

namespace ServiceAbstraction
{
    public interface IAvatarService
    {
        Task<IEnumerable<AvatarDto>> GetAllAvatarsAsync();
    }
}
