using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Localization;
using ServiceAbstraction;
using Shared.Const;
using Shared.DTOs;
using Shared.DTOs.Wardrobe;
using Shared.Exceptions;
using Shared.SharedResources;

namespace Service
{
    public class WardrobeService(
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
        : IWardrobeService
    {
        public async Task<Result<int>> AddItemAsync(string userId, CreateWardrobeItemWithAttributesRequest request)
        {
            var wardrobeRepo = unitOfWork.GetRepository<Wardrobe, int>();
            var itemRepo = unitOfWork.GetRepository<WardrobeItem, int>();

            var wardrobe = await wardrobeRepo.GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId);

            if (wardrobe is null)
            {
                wardrobe = new Wardrobe(userId);
                await wardrobeRepo.AddAsync(wardrobe);
            }

            var imageUrl = await fileStorageService.SaveFileAsync(request.Image, WardrobeItemConst.FolderPath, WardrobeItemConst.FileMaxSize);

            ItemAttribute? attributes = null;

            if (request.Attributes is not null)
            {
                attributes = new ItemAttribute
                {
                    Category = request.Attributes.Category,
                    Color = request.Attributes.Color,
                    Fit = request.Attributes.Fit,
                    Style = request.Attributes.Style,
                    Season = request.Attributes.Season,
                    Pattern = request.Attributes.Pattern
                };
            }

            var item = new WardrobeItem(imageUrl, wardrobe.Id, attributes);
            await itemRepo.AddAsync(item);

            await unitOfWork.SaveChangesAsync();

            return Result<int>.Success(item.Id, localizer[SharedResourcesKeys.WardrobeItemCreatedSuccessfully]);
        }

        public async Task<Result<int>> AddItemToMyClosetAsync(string userId, CreateWardrobeItemRequest request)
        {
            var wardrobeRepo = unitOfWork.GetRepository<Wardrobe, int>();
            var itemRepo = unitOfWork.GetRepository<WardrobeItem, int>();

            var wardrobe = await wardrobeRepo.GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId);

            if (wardrobe is null)
            {
                wardrobe = new Wardrobe(userId);
                await wardrobeRepo.AddAsync(wardrobe);
                await unitOfWork.SaveChangesAsync();
            }

            var imageUrl = await fileStorageService.SaveFileAsync(request.Image, WardrobeItemConst.FolderPath, WardrobeItemConst.FileMaxSize);

            var item = new WardrobeItem(imageUrl, wardrobe.Id);
            await itemRepo.AddAsync(item);

            await unitOfWork.SaveChangesAsync();

            return Result<int>.Success(item.Id, localizer[SharedResourcesKeys.WardrobeItemAddedToYourClosetSuccessfully]);
        }

        public async Task<Result<WardrobeDto>> GetMyClosetAsync(string userId)
        {
            var repo = unitOfWork.GetRepository<Wardrobe, int>();

            var wardrobe = await repo.GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId, x => x.Items) ??
                throw new NotFoundException(localizer[SharedResourcesKeys.WardrobeNotFound]);

            var result = new WardrobeDto
            {
                Id = wardrobe.Id,

                Items = wardrobe.Items.Select(item => new WardrobeItemDto
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    CreatedAt = item.CreatedAt,

                    Attributes = item.Attributes is null
                        ? null
                        : new ItemAttributeDto
                        {
                            Category = item.Attributes.Category,
                            Color = item.Attributes.Color,
                            Fit = item.Attributes.Fit,
                            Style = item.Attributes.Style,
                            Season = item.Attributes.Season,
                            Pattern = item.Attributes.Pattern
                        }
                })
            };

            return Result<WardrobeDto>.Success(result, localizer[SharedResourcesKeys.WardrobeRetrievedSuccessfully]);
        }
    }
}