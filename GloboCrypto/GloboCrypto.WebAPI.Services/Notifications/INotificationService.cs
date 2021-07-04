﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboCrypto.Models.Notifications;

namespace GloboCrypto.WebAPI.Services.Notifications
{
    public interface INotificationService
    {
        Task CheckAndNotifyAsync();
        Task<NotificationSubscription> SubscribeAsync(string userId, NotificationSubscription subscription);
        Task UpdateSubscriptionAsync(string userId, string coinIds);
    }
}
