using GiftKnackNotificationAgent.Models;
using GiftKnackProject.NotificationTypes;

namespace GiftKnackNotificationAgent.Services
{
    public interface INotificationHandlerFactory
    {
        IBaseNotificationHandler<TInputMessage> Resolve<TInputMessage>() where TInputMessage : BaseQueueNotification;
    }
}
