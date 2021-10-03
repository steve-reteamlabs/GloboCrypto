using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboCrypto.Models.Notifications;
using GloboCrypto.WebAPI.Services.Notifications;
using Microsoft.AspNetCore.Authorization;

namespace GloboCrypto.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService NotificationService;
        public NotificationController(INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        [HttpPut("subscribe")]
        [Authorize]
        public async Task<NotificationSubscription> Subscribe(NotificationSubscription subscription)
        {
            return await NotificationService.SubscribeAsync(User.Identity.Name, subscription);
        }

        [HttpGet("update-subscription")]
        [Authorize]
        public async Task UpdateSubscription(string coinIds)
        {
            await NotificationService.UpdateSubscriptionAsync(User.Identity.Name, coinIds);
        }

        [HttpGet("check-and-notify")]
        [Authorize]
        public async Task CheckAndNotify()
        {
            await NotificationService.CheckAndNotifyAsync();
        }

    }
}
