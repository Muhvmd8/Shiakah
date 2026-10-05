using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Options;
using ServiceAbstraction;
using Shared.DTOs.Avatar;
using Shared.Settings;

namespace Service
{
    public class AvatarService(IUnitOfWork unitOfWork,IOptions<AppSettings> appSettings) : IAvatarService
    {
        public async Task<IEnumerable<AvatarDto>> GetAllAvatarsAsync()
        {
            var repo = unitOfWork.GetRepository<Avatar, int>();
            var avatars=await repo.GetAllAsync();
            var baseUrl=appSettings.Value.BaseUrl;
            return avatars.Select(x => new AvatarDto
            {
                Id = x.Id,
                Url = baseUrl+ x.Url
            });
        }
    }
}
