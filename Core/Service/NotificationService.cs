using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Service.Helpers;
using ServiceAbstraction;
using Shared.DTOs.Notification;
using Shared.SharedResources;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRealTimeSender _realTimeSender;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;

        public NotificationService(
            IUnitOfWork unitOfWork,
            INotificationRealTimeSender realTimeSender,
            IStringLocalizer<SharedResources> localizer,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _realTimeSender = realTimeSender;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task SendAsync(NotificationCreateDto dto)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();

            var notification = NotificationFactoryHelper.Create(
                dto.UserId,
                dto.Type,
                dto.Message,
                dto.Data
            );

            await repo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _realTimeSender.SendToUserAsync(dto.UserId, new
            {
                notification.Id,
                notification.Message,
                notification.Type,
                notification.CreatedAt,
                notification.DataJson
            });
        }

        //public async Task<PaginatedResultDto<NotificationDto>> GetUserNotificationsPagedAsync(
        //    string userId, int pageNumber, int pageSize)
        //{
        //    var repo = _unitOfWork.GetRepository<Notification, int>();

        //    var query = repo.GetAllQueryable(n => n.ApplicationUserId == userId)
        //                    .OrderByDescending(n => n.CreatedAt);

        //    var totalCount = await query.CountAsync();

        //    var notifications = await query.Skip((pageNumber - 1) * pageSize)
        //                                   .Take(pageSize)
        //                                   .ToListAsync();

        //    var mapped = _mapper.Map<List<NotificationDto>>(notifications);

        //    return new PaginatedResultDto<NotificationDto>(mapped, totalCount, pageNumber, pageSize);
        //}

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            return await repo.CountAsync(n => n.ApplicationUserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int id, string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var notif = await repo.GetByIdAsync(id);

            if (notif == null || notif.ApplicationUserId != userId)
                throw new Exception("Notification not found.");

            notif.IsRead = true;
            repo.Update(notif);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAllReadAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();

            var unread = await repo.GetAllQueryable(n => n.ApplicationUserId == userId && !n.IsRead)
                                   .ToListAsync();

            foreach (var n in unread)
                n.IsRead = true;

            repo.UpdateRange(unread);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(int id, string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var notif = await repo.GetByIdAsync(id);

            if (notif == null || notif.ApplicationUserId != userId)
                throw new Exception("Notification not found");

            repo.Remove(notif);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
