using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnackNotificationAgent.Models;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Microsoft.ServiceBus.Messaging;

namespace GiftKnackNotificationAgent.Services
{
    public interface IMessageFromMqProcessor
    {
        Task<IEnumerable<Notification>> ProcessMessage(BrokeredMessage message);
    }
}