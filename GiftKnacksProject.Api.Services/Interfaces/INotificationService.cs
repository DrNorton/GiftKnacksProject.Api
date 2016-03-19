using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace GiftKnacksProject.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task SentNotificationToQueue(BaseQueueNotification activity);
        Task<List<Notification>> GetLenta(long id);
    }
}