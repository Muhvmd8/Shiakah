using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.OutfitResults;
using Shared.DTOs.Wardrobe;
using Shared.Exceptions;
using Shared.SharedResources;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Service
{
    public class OutfitResultService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer,
        IWardrobeService wardrobeService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
        : IOutfitResultService
    {
        public async Task<Result<OutfitResultDto>> GenerateAsync(string userId, GenerateOutfitRequest request)
        {
            var newWardrobeItem = new CreateWardrobeItemWithAttributesRequest
            {
                Image = request.Image,
                Attributes = request.Attributes
            };

            var wardrobeItemResult = await wardrobeService.AddItemAsync(userId, newWardrobeItem);

            if (!wardrobeItemResult.Succeeded)
                return Result<OutfitResultDto>.Fail(wardrobeItemResult.Message);

            var aiResult = await CallOutfitModelAsync(request);

            var outfitResult = new OutfitResult(
                userId,
                aiResult.Response.Items.Top,
                aiResult.Response.Items.Bottom,
                aiResult.Response.Items.Footwear,
                aiResult.Response.Items.Accessories,
                aiResult.Response.RecommendedImageUrl,
                aiResult.Response.SearchQuery,
                aiResult.Response.StylingTips,
                aiResult.Response.Summary);

            var outfitRepository = unitOfWork.GetRepository<OutfitResult, int>();

            await outfitRepository.AddAsync(outfitResult);
            await unitOfWork.SaveChangesAsync();

            var result = new OutfitResultDto
            {
                Id = outfitResult.Id,
                Top = outfitResult.Top,
                Bottom = outfitResult.Bottom,
                Footwear = outfitResult.Footwear,
                Accessories = outfitResult.Accessories,
                RecommendedImageUrl = outfitResult.RecommendedImageUrl,
                SearchQuery = outfitResult.SearchQuery,
                StylingTips = outfitResult.StylingTips,
                Summary = outfitResult.Summary,
                CreatedAt = outfitResult.CreatedAt
            };

            return Result<OutfitResultDto>.Success(
                result,
                localizer[SharedResourcesKeys.OutfitResultCreatedSuccessfully]);
        }

        public async Task<Result<IEnumerable<OutfitResultDto>>> GetAllAsync()
        {
            var repo = unitOfWork.GetRepository<OutfitResult, int>();
            var outfitResults = await repo.GetAllAsync();

            var result = outfitResults
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new OutfitResultDto
                {
                    Id = x.Id,
                    Top = x.Top,
                    Bottom = x.Bottom,
                    Footwear = x.Footwear,
                    Accessories = x.Accessories,
                    RecommendedImageUrl = x.RecommendedImageUrl,
                    SearchQuery = x.SearchQuery,
                    StylingTips = x.StylingTips,
                    Summary = x.Summary,
                    CreatedAt = x.CreatedAt
                });

            return Result<IEnumerable<OutfitResultDto>>.Success(
                result,
                localizer[SharedResourcesKeys.OutfitResultsRetrievedSuccessfully]);
        }

        public async Task<Result<IEnumerable<OutfitResultDto>>> GetAllForUserAsync(string userId)
        {
            var repo = unitOfWork.GetRepository<OutfitResult, int>();
            var outfitResults = await repo.GetAllAsync(x => x.ApplicationUserId == userId);

            var result = outfitResults
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new OutfitResultDto
                {
                    Id = x.Id,
                    Top = x.Top,
                    Bottom = x.Bottom,
                    Footwear = x.Footwear,
                    Accessories = x.Accessories,
                    RecommendedImageUrl = x.RecommendedImageUrl,
                    SearchQuery = x.SearchQuery,
                    StylingTips = x.StylingTips,
                    Summary = x.Summary,
                    CreatedAt = x.CreatedAt
                });

            return Result<IEnumerable<OutfitResultDto>>.Success(
                result,
                localizer[SharedResourcesKeys.OutfitResultsRetrievedSuccessfully]);
        }

        private async Task<OutfitAiResponseDto> CallOutfitModelAsync(GenerateOutfitRequest request)
        {
            var client = httpClientFactory.CreateClient();
            var endpoint = configuration["OutfitAiUrl"];

            using var content = new MultipartFormDataContent();
            using var imageStream = request.Image.OpenReadStream();
            using var imageContent = new StreamContent(imageStream);

            imageContent.Headers.ContentType = new MediaTypeHeaderValue(
                request.Image.ContentType ?? "application/octet-stream");

            content.Add(imageContent, "image", request.Image.FileName);
            content.Add(new StringContent(request.Attributes.Event), "meeting_type");
            content.Add(new StringContent(request.Attributes.Style), "preferred_styles");
            content.Add(new StringContent(request.Attributes.Location), "location");

            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(endpoint, content);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(
                    $"Failed to reach Outfit AI service: {ex.Message}");
            }

            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(
                    $"AI service returned {(int)response.StatusCode}: {raw}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<OutfitAiResponseDto>(raw, options);

            if (result is null || result.Response is null)
                throw new BadRequestException(localizer[SharedResourcesKeys.AIResultFailed]);

            return result;
        }
    }
}