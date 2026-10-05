//using ServiceAbstraction;

//namespace Presentation.Controllers
//{
//    [Authorize]
//    public class NotificationController(INotificationService notificationService) : ApiController
//    {
//        //[HttpGet]
//        //public async Task<IActionResult> GetNotifications(
//        //    [FromQuery] int pageNumber = 1,
//        //    [FromQuery] int pageSize = 20)
//        //{
//        //    var userId = GetIdFromToken();
//        //    var result = await notificationService.GetUserNotificationsPagedAsync(userId, pageNumber, pageSize);
//        //    return Ok(result);
//        //}

//        [HttpGet("unread-count")]
//        public async Task<IActionResult> GetUnreadCount()
//        {
//            var userId = GetIdFromToken();
//            var count = await notificationService.GetUnreadCountAsync(userId);
//            return Ok(new { count });
//        }

//        [HttpPost("{id}/read")]
//        public async Task<IActionResult> MarkAsRead(int id)
//        {
//            var userId = GetIdFromToken();
//            await notificationService.MarkAsReadAsync(id, userId);
//            return Ok();
//        }

//        [HttpPost("mark-all-read")]
//        public async Task<IActionResult> MarkAllRead()
//        {
//            var userId = GetIdFromToken();
//            await notificationService.MarkAllReadAsync(userId);
//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var userId = GetIdFromToken();
//            await notificationService.DeleteNotificationAsync(id, userId);
//            return Ok();
//        }
//    }
//}