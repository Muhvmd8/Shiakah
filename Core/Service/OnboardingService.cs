using Domain.Contracts;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.OnBoarding;
using Shared.Enums;
using Shared.Exceptions;
using Shared.SharedResources;

namespace Service
{
    public class OnboardingService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
        : IOnboardingService
    {
        public async Task<Result> UpdateGenderAsync(string userId, UpdateGenderRequest request)
        {
            var user = await GetCurrentUserAsync(userId);
            var profile = await GetOrCreateProfileAsync(user.Id);

            profile.Gender = request.Gender;
            user.OnboardingStep = OnboardingStep.Appearance;

            await unitOfWork.SaveChangesAsync();

            return Result.Success(localizer[SharedResourcesKeys.GenderUpdatedSuccessfully]);
        }

        public async Task<Result> UpdateAppearanceAsync(string userId, UpdateAppearanceRequest request)
        {
            var profile = await GetOrCreateProfileAsync(userId);

            profile.AgeRange = request.AgeRange;
            profile.SkinTone = request.SkinTone;
            profile.ApplicationUser.OnboardingStep = OnboardingStep.Body;

            await unitOfWork.SaveChangesAsync();

            return Result.Success(localizer[SharedResourcesKeys.AppearanceUpdatedSuccessfully]);
        }

        public async Task<Result> UpdateBodyAsync(string userId, UpdateBodyRequest request)
        {
            var user = await GetCurrentUserAsync(userId);
            var profile = await GetOrCreateProfileAsync(user.Id);

            profile.Height = request.Height;
            profile.Weight = request.Weight;
            profile.BodyType = request.BodyType;

            user.OnboardingStep = OnboardingStep.Preferences;

            await unitOfWork.SaveChangesAsync();

            return Result.Success(localizer[SharedResourcesKeys.BodyUpdatedSuccessfully]);
        }

        public async Task<Result> UpdatePreferencesAsync(string userId, UpdatePreferencesRequest request)
        {
            var user = await GetCurrentUserAsync(userId);
            var userProfile = await GetProfileAsync(user.Id);

            var styleRepo = unitOfWork.GetRepository<UserProfileStyle, int>();
            var colorRepo = unitOfWork.GetRepository<UserProfilePreferredColor, int>();

            var existingStyles = await styleRepo
                .GetAllQueryable(x => x.UserProfileId == userProfile.Id)
                .ToListAsync();

            var existingColors = await colorRepo
                .GetAllQueryable(x => x.UserProfileId == userProfile.Id)
                .ToListAsync();

            if (existingStyles.Count > 0)
                styleRepo.RemoveRange(existingStyles);

            if (existingColors.Count > 0)
                colorRepo.RemoveRange(existingColors);

            foreach (var style in request.Styles.Distinct())
                await styleRepo.AddAsync(new UserProfileStyle
                {
                    UserProfileId = userProfile.Id,
                    Style = style
                });

            foreach (var color in request.PreferredColors.Distinct())
                await colorRepo.AddAsync(new UserProfilePreferredColor
                {
                    UserProfileId = userProfile.Id,
                    Color = color
                });

            user.OnboardingStep = OnboardingStep.Profile;

            await unitOfWork.SaveChangesAsync();

            return Result.Success(localizer[SharedResourcesKeys.PreferencesUpdatedSuccessfully]);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await GetCurrentUserAsync(userId);
            var profile = await GetOrCreateProfileAsync(user.Id);

            profile.Name = request.Name;
            profile.Location = request.Location;

            user.OnboardingStep = OnboardingStep.Completed;
            user.OnboardingCompleted = true;

            await unitOfWork.SaveChangesAsync();

            return Result.Success(localizer[SharedResourcesKeys.OnboardingCompletedSuccessfully]);
        }

        public async Task<Result<OnboardingStatusResponse>> GetOnboardingStatusAsync(string userId)
        {
            var user = await GetCurrentUserAsync(userId);
            var profile = await GetProfileAsync(user.Id);

            var response = new OnboardingStatusResponse
            {
                CurrentStep = user.OnboardingStep,
                Completed = user.OnboardingStep == OnboardingStep.Completed,
                Profile = new OnboardingProfileResponse
                {
                    Gender = profile.Gender.ToString(),
                    AgeRange = profile.AgeRange.ToString(),
                    SkinTone = profile.SkinTone.ToString(),
                    Height = profile.Height,
                    Weight = profile.Weight,
                    BodyType = profile.BodyType.ToString(),
                    Name = profile.Name,
                    Location = profile.Location
                }
            };

            return Result<OnboardingStatusResponse>.Success(response, SharedResourcesKeys.OnboardingStatusRetrievedSuccessfully);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync(string userId)
        {
            // Replace this with your existing way
            // of getting the current authenticated user.

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedException(localizer[SharedResourcesKeys.Unauthorized]);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId) ??
                throw new NotFoundException(localizer[SharedResourcesKeys.UserNotFound]);

            return user;
        }

        private async Task<UserProfile> GetProfileAsync(string userId)
        {
            var profileRepo = unitOfWork.GetRepository<UserProfile, int>();
            return await profileRepo.GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId) ??
                throw new NotFoundException(localizer[SharedResourcesKeys.ProfileNotFound]);
        }

        private async Task<UserProfile> GetOrCreateProfileAsync(string userId)
        {
            var profileRepo = unitOfWork.GetRepository<UserProfile, int>();

            var profile = await profileRepo.GetFirstOrDefaultAsync(x => x.ApplicationUserId == userId, u => u.ApplicationUser);

            if (profile != null) return profile;

            profile = new UserProfile
            {
                ApplicationUserId = userId,
            };

            await profileRepo.AddAsync(profile);

            return profile;
        }
    }
}