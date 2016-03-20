using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace GiftKnackMessageAgent.Services
{
    public interface IChatMessageFromMqProcessor
    {
        Task ProcessMessage(BrokeredMessage brokeredMessage);
    }
}