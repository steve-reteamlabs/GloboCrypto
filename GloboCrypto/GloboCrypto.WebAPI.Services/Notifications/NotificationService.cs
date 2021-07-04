using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboCrypto.Models.Data;
using GloboCrypto.Models.Notifications;
using GloboCrypto.WebAPI.Services.Data;
using GloboCrypto.WebAPI.Services.Events;

namespace GloboCrypto.WebAPI.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ILocalDbService LocalDb;
        private readonly IEventService EventService;

        public NotificationService(ILocalDbService localDb, IEventService eventService)
        {
            LocalDb = localDb;
            EventService = eventService;
        }

        public async Task<NotificationSubscription> SubscribeAsync(string userId, NotificationSubscription subscription)
        {
            LocalDb.Delete<NotificationSubscription>(e => e.UserId == userId);

            subscription.UserId = userId;
            await Task.Run(() => LocalDb.Upsert(subscription));

            await EventService.LogSubscription(userId);

            return subscription;
        }

        public async Task CheckAndNotifyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSubscriptionAsync(string userId, string coinIds)
        {
            await Task.Run(() =>
            {
                var subscription = LocalDb.Query<NotificationSubscription>(sub => sub.UserId == userId).FirstOrDefault();
                if (subscription != null)
                {
                    var coins = coinIds?.Split(',').ToList();
                    subscription.CoinIds = coins;
                    LocalDb.Upsert(subscription);
                    EventService.LogSubscriptionUpdate(userId);
                }
            });
        }

    }
}
