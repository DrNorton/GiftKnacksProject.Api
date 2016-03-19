using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnackNotificationAgent.Models
{

    public interface ICommandHandlerFactory
    {
        IBaseNotificationHandler<InputMessage> Resolve<InputMessage>() where InputMessage : BaseQueueNotification;
    }


    public interface IBaseNotificationHandler<T> where T : BaseQueueNotification
    {
        Task<IEnumerable<Notification>> Handle(T messageFromQueue);
    }

    public abstract class BaseNotificationHandler<T> : IBaseNotificationHandler<T> where T:BaseQueueNotification
    {
        private List<Notification> _resultNotification;
        public abstract bool IsMultipleNotification { get; }

        public BaseNotificationHandler()
        {
            _resultNotification=new List<Notification>();
        }

        public async Task<IEnumerable<Notification>> Handle(T messageFromQueue)
        {
            if (!IsMultipleNotification)
            {
                var notification = new Notification();
                notification.Action = messageFromQueue.Type;
                notification.Time = messageFromQueue.NotificationTime;
                notification.Info = await ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(messageFromQueue);
                _resultNotification.Add(notification);

            }
            else
            {
                var infos = await ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(messageFromQueue);
                foreach (var info in infos)
                {
                    var notification=new Notification();
                    notification.Action = messageFromQueue.Type;
                    notification.Time = messageFromQueue.NotificationTime;
                    notification.Info = info;
                    _resultNotification.Add(notification);
                }
            }


            return _resultNotification;

        }

        public abstract Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(T messageFromQueue);
        public abstract Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(T messageFromQueue);


    }
}
