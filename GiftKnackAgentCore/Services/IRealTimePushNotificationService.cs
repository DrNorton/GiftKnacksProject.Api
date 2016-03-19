using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;

namespace GiftKnackAgentCore.Services
{
    public interface IRealTimePushNotificationService
    {
        Task SentRealTimeMessages(IEnumerable<Notification> notifications);
    }
}