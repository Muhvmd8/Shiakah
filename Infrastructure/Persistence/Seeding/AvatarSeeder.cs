using Domain.Contracts;
using Domain.Models;

namespace Persistence.Seeding
{
    public class AvatarSeeder
    {
        private readonly IUnitOfWork _unitOfWork;

        public AvatarSeeder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SeedAsync()
        {
            var repo = _unitOfWork.GetRepository<Avatar, int>();

            var avatars = new List<Avatar>
            {
                new Avatar { Url = "Images/Avatars/avatar-1-watermelon.png" },
                new Avatar { Url = "Images/Avatars/avatar-2-orange.png" },
                new Avatar { Url = "Images/Avatars/avatar-3-apple.png" },
                new Avatar { Url = "Images/Avatars/avatar-4-banana.png" },
                new Avatar { Url = "Images/Avatars/avatar-5-strawberry.png" },
                new Avatar { Url = "Images/Avatars/avatar-6-grapes.png" },
            };

            foreach (var avatar in avatars)
            {
                var exists = await repo.GetFirstOrDefaultAsync(a => a.Url == avatar.Url);
                if (exists == null)
                    await repo.AddAsync(avatar);
            }

        }
    }
}
