using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Managers.Abstract;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationManager _notificationManager;

        public NotificationsController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        // POST api/notifications/send
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _notificationManager.SendNotificationAsync(request.Token, request.Title, request.Body);
                return Ok("Bildirim gönderildi.");
            }
            catch (Exception ex)
            {
                // Gerçek uygulamalarda hata loglaması yapılmalıdır.
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }
        
        [HttpPost("sendToAll")]
        public async Task<IActionResult> SendNotificationToAll([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _notificationManager.SendNotificationToTopicAsync("allUsers", request.Title, request.Body);
                return Ok("Tüm kullanıcılara bildirim gönderildi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }

    }

    public class NotificationRequest
    {
        public string Token { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}