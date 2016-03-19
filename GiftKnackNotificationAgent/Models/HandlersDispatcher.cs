using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackNotificationAgent.Models.Handlers;
using GiftKnackNotificationAgent.Services;
using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Newtonsoft.Json;

namespace GiftKnackNotificationAgent.Models
{
    public interface IHandlersDispatcher
    {
        Task<IEnumerable<Notification>> FindHandlerAndExecute<T>(string messageBody) where  T:BaseQueueNotification;
    }

    public class HandlersDispatcher : IHandlersDispatcher
    {
        private readonly INotificationHandlerFactory _handlerFactory;

        public HandlersDispatcher(INotificationHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        public Task<IEnumerable<Notification>> FindHandlerAndExecute<T>(string messageBody) where  T:BaseQueueNotification
        {
            var addCommentNotificationHandler = _handlerFactory.Resolve<T>();
            return addCommentNotificationHandler.Handle(JsonConvert.DeserializeObject<T>(messageBody));
        }
    }
}
