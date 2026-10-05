//using Domain.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Localization;
//using Microsoft.Extensions.Options;
//using ServiceAbstraction;
//using Shared.DTOs.General;
//using Shared.DTOs.User;
//using Shared.Enums;
//using Shared.Settings;
//using Shared.SharedResources;
//namespace Service
//{
//    public class UserService(UserManager<ApplicationUser> userManager,
//        IStringLocalizer<SharedResources> localizer,
//        IOptions<AppSettings> appSettings,
//        IFileStorageService fileStorageService)
//        : IUserService
//    {
//        public async Task<PaginatedResultDto<UserResponse>> GetUsersByNameAsync(
//            string name, int pageSize, int pageNumber, string currentUserId)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                return new PaginatedResultDto<UserResponse>([], 0, pageNumber, pageSize);

//            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

//            var baseUrl = appSettings.Value.BaseUrl?.TrimEnd('/');

//            var excludedIds = new HashSet<string>();

//            foreach (var roleName in new[] { Roles.Admin.ToString(), Roles.SuperAdmin.ToString() })
//            {
//                var usersInRole = await userManager.GetUsersInRoleAsync(roleName);
//                foreach (var u in usersInRole)
//                    excludedIds.Add(u.Id);
//            }

//            var excludedIdsList = excludedIds.ToList();

//            var filteredQuery = userManager.Users
//                .Where(u =>
//                    u.EmailConfirmed &&
//                    u.Id != currentUserId &&
//                    u.Name != null &&
//                    u.Name.ToLower().Contains(name.ToLower()) &&
//                    !excludedIdsList.Contains(u.Id)
//                );

//            var totalCount = await filteredQuery.CountAsync();

//            var pagedIds = await filteredQuery
//                .OrderBy(u => u.Name)
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .Select(u => u.Id)
//                .ToListAsync();

//            if (!pagedIds.Any())
//                return new PaginatedResultDto<UserResponse>([], totalCount, pageNumber, pageSize);

//            var users = await userManager.Users
//                .Where(u => pagedIds.Contains(u.Id))
//                .Include(u => u.Avatar)
//                .ToListAsync();

//            var result = users.Select(u => new UserResponse
//            {
//                Id = u.Id,
//                Name = u.Name!,
//                UserName = u.UserName!,
//                ProfilePicture = string.IsNullOrWhiteSpace(u.ProfileImageUrl)
//                    ? $"{baseUrl}/{u.Avatar.Url}"
//                    : $"{baseUrl}/{u.ProfileImageUrl}",
//            })
//            .OrderBy(u => u.Name)
//            .ToList();

//            return new PaginatedResultDto<UserResponse>(result, totalCount, pageNumber, pageSize);
//        }
//        public async Task<UserByUsernameResult> GetUserByUsernameAsync(string username, string currentUserId)
//        {
//            var baseUrl = appSettings.Value.BaseUrl?.TrimEnd('/');

//            var user = await userManager.Users
//                .Where(u =>
//                    u.EmailConfirmed &&
//                    u.UserName != null &&
//                    u.UserName.ToLower() == username.ToLower())
//                .Include(u => u.Avatar)
//                .FirstOrDefaultAsync();

//            if (user == null)
//                throw new Exception(localizer[SharedResourcesKeys.UserNotFound]);

//            var result = new UserByUsernameResult
//            {
//                Id = user.Id,
//                Name = user.Name!,
//                Username = user.UserName!,
//                ProfilePicture = string.IsNullOrWhiteSpace(user.ProfileImageUrl)
//                    ? $"{baseUrl}/{user.Avatar.Url}"
//                    : $"{baseUrl}/{user.ProfileImageUrl}",
//                AboutMe = user.AboutMe
//            };

//            return result;
//        }
//        public async Task<MyProfileResponse> GetMyProfileAsync(string userId)
//        {
//            var user = await userManager.Users
//                .Include(u => u.Avatar)
//                .FirstOrDefaultAsync(u => u.Id == userId && u.EmailConfirmed);

//            if (user == null)
//                throw new Exception(localizer[SharedResourcesKeys.UserNotFound]);

//            var baseUrl = appSettings.Value.BaseUrl?.TrimEnd('/');

//            var profilePic = string.IsNullOrWhiteSpace(user.ProfileImageUrl)
//                ? $"{baseUrl}/{user.Avatar.Url}"
//                : $"{baseUrl}/{user.ProfileImageUrl}";


//            var response = new MyProfileResponse
//            {
//                Id = user.Id,
//                Name = user.Name ?? string.Empty,
//                Username = user.UserName ?? string.Empty,
//                Email = user.Email ?? string.Empty,
//                ProfilePicture = profilePic,
//                AboutMe = user.AboutMe,
//                DateOfBirth = user.DateOfBirth,
//                Gender = user.Gender
//            };

//            return response;
//        }
//        public async Task<MyProfileResponse> UpdateMyProfileAsync(string userId, UpdateUserDto dto)
//        {
//            var user = await userManager.Users
//                .Where(u => u.Id == userId && u.EmailConfirmed)
//                .Include(u => u.Avatar)
//                .FirstOrDefaultAsync();

//            if (user == null)
//                throw new Exception(localizer[SharedResourcesKeys.UserNotFound]);

//            if (dto.ProfileImage is not null && dto.AvatarId is not null)
//                throw new Exception(localizer[SharedResourcesKeys.OnlyOneImageCanBeUsed]);

//            user.Name = dto.Name;

//            user.AboutMe = dto.AboutMe;

//            if (dto.Gender is not null)
//                user.Gender = dto.Gender.Value;
//            if (dto.DateOfBirth is not null)
//                user.DateOfBirth = dto.DateOfBirth;

//            string? newImageUrl = null;

//            if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
//            {
//                var directory = "Images/ProfileImages";

//                if (!string.IsNullOrWhiteSpace(user.ProfileImageUrl))
//                    await fileStorageService.DeleteFileAsync(user.ProfileImageUrl);

//                newImageUrl = await fileStorageService
//                    .SaveFileAsync(dto.ProfileImage, directory, maxSize: 5 * 1024 * 1024);

//                user.ProfileImageUrl = newImageUrl;
//                user.AvatarId = null;
//            }
//            else if (dto.AvatarId is not null)
//            {
//                user.AvatarId = dto.AvatarId;
//                if (user.ProfileImageUrl is not null)
//                {
//                    await fileStorageService.DeleteFileAsync(user.ProfileImageUrl);
//                    user.ProfileImageUrl = null;
//                }
//            }

//            var result = await userManager.UpdateAsync(user);
//            if (!result.Succeeded)
//            {
//                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
//                throw new Exception(SharedResourcesKeys.AnErrorOccured);
//            }

//            var baseUrl = appSettings.Value.BaseUrl?.TrimEnd('/');
//            var profileUrl = "";

//            if (!string.IsNullOrWhiteSpace(user.ProfileImageUrl))
//            {
//                profileUrl = $"{baseUrl}/{user.ProfileImageUrl}";
//            }
//            else if (user.Avatar != null)
//            {
//                profileUrl = $"{baseUrl}/{user.Avatar.Url}";
//            }
//            else
//            {
//                profileUrl = $"{baseUrl}/Images/profileImages/default.png";
//            }

//            return new MyProfileResponse
//            {
//                Id = user.Id,
//                Name = user.Name,
//                Username = user.UserName ?? "",
//                Email = user.Email ?? "",
//                ProfilePicture = profileUrl,
//                AboutMe = user.AboutMe,
//                DateOfBirth = user.DateOfBirth,
//                Gender = user.Gender
//            };
//        }
//    }

//}