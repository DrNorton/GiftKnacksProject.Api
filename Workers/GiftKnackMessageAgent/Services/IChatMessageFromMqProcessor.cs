using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.Chat;
using Microsoft.ServiceBus.Messaging;

namespace GiftKnackMessageAgent.Services
{
    public interface IChatMessageFromMqProcessor
    {
        Task<ChatMqMessage> ProcessMessage(BrokeredMessage brokeredMessage);
    }
}