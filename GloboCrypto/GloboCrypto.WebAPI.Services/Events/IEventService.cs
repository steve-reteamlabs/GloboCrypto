using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using GloboCrypto.WebAPI.Services.Models;

namespace GloboCrypto.WebAPI.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventLogEntry>> GetAllEvents();
        Task<IEnumerable<EventLogEntry>> GetEventsByTypes(IEnumerable<EventLogEntryType> entryTypes);
        Task LogAuthentication(string userId);
        Task LogSubscription(string userId);
        Task LogSubscriptionUpdate(string userId);
        Task LogError(string message, Exception ex);
        Task LogCoinUpdateNotification(string userId, string coinId);
        Task<IEnumerable<EventLogEntry>> QueryEvents(Expression<Func<EventLogEntry, bool>> query);

    }
}
