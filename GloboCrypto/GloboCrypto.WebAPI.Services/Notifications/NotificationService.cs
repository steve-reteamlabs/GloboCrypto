using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GloboCrypto.Models.Data;
using GloboCrypto.Models.Notifications;
using GloboCrypto.WebAPI.Services.Coins;
using GloboCrypto.WebAPI.Services.Data;
using GloboCrypto.WebAPI.Services.Events;
using WebPush;

namespace GloboCrypto.WebAPI.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ILocalDbService LocalDb;
        private readonly IEventService EventService;
        private readonly ICoinService CoinService;

        public NotificationService(ILocalDbService localDb, IEventService eventService, ICoinService coinService)
        {
            LocalDb = localDb;
            EventService = eventService;
            CoinService = coinService;
        }

        public async Task SendAsync(string coinId, string message)
        {
            var subject = "mailto:steve-reteamlabs@gmail.com";
            var publicKey = "BBCdWc-g92B4hcMCLwlZAEs49k52NzvcJiLIfrkGmulGm2-HIcduBXbHvA7WwkbJLz3nCGwCjpI7pkZkF3Qt3So";
            var privateKey = "WfeZokELwNc_uh0HxWGqPr_aAJ80FWNTvQdAO8bkfyo";

            var coinInfo = await CoinService.GetCoinInfo(coinId);
            var subs = LocalDb.Query<NotificationSubscription>(sub => sub.CoinIds.Contains(coinId)).ToList();

            foreach (var subscription in subs)
            {
                var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var webPushClient = new WebPushClient();
                try
                {
                    var payload = JsonSerializer.Serialize(new
                    {
                        message,
                        url = $"/",
                        iconurl = coinInfo.LogoUrl,
                    });
                    await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                }
                catch (WebPushException ex)
                {
                }
            }
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
            const string INTERVAL = "1d";
            var allCoinIds = string.Join(",", LocalDb.All<NotificationSubscription>().Select(sub => string.Join(",", sub.CoinIds.ToArray())));
            var uniqueCoinIds = string.Join(",", allCoinIds.Split(',').Distinct());
            var allInfo = await CoinService.GetCoinPriceInfo(uniqueCoinIds, "GBP", INTERVAL);
            foreach (var coinPriceInfo in allInfo)
            {
                var priceChangePctRaw = coinPriceInfo.Intervals[INTERVAL].PriceChangePct;
                var priceChangePct = Math.Abs(float.Parse(coinPriceInfo.Intervals[INTERVAL].PriceChangePct));
                if (priceChangePct > 0.05)
                {
                    await SendAsync(coinPriceInfo.Id, $"{coinPriceInfo.Id} has changed {(priceChangePct * 100):0.00} in the last hour");
                }
            }
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
