using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Options;
using Shared.DTOs.Category;
using Shared.DTOs.Notification;
using Shared.Settings;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { }
        public MappingProfile(IOptions<AppSettings> appSettings)
        {
            var baseUrl = appSettings.Value.BaseUrl?.TrimEnd('/');

            #region Category
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => baseUrl + '/' + src.IconUrl));
            CreateMap<Category, CategoryDetailedDto>()
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => baseUrl + '/' + src.IconUrl));
            #endregion

            #region Notification
            CreateMap<Notification, NotificationDto>();
            #endregion
        }
    }
}