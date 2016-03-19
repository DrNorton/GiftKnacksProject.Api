using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackAgentCore.Services;
using GiftKnackNotificationAgent.Models;

using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiftKnackNotificationAgent.Services
{
    public class MessageFromMqProcessor : IMessageFromMqProcessor
    {
        private readonly IHandlersDispatcher _handlersDispatcher;
        private readonly INoSqlDatabaseRepository _noSqlDatabaseRepository;
        public MessageFromMqProcessor(IHandlersDispatcher handlersDispatcher, INoSqlDatabaseRepository noSqlDatabaseRepository)
        {
            _handlersDispatcher = handlersDispatcher;
            _noSqlDatabaseRepository = noSqlDatabaseRepository;
        }

        public async Task<IEnumerable<Notification>> ProcessMessage(BrokeredMessage message)
        {
            var type = message.Properties["Type"].ToString().ToLower();
            var body=message.GetBody<string>();
            IEnumerable<Notification> notifications=null;
            
            switch (type)
            {
                case "addcomment":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<AddCommentQueueNotification>(body);
                     
                    break;

                case "join":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<JoinQueueNotification>(body);
                    break;


                case "addreference":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<AddReferenceQueueNotification>(body);
                    break;

                case "closejoineditem":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<CloseItemQueueNotification>(body);
                    break;

                case "totalclosechange":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<TotalClosedQueueNotification>(body);
                    break;

                case "commentreply":
                    notifications = await _handlersDispatcher.FindHandlerAndExecute<ReplyToCommentQueueNotification>(body);
                    break;

            }

            await  _noSqlDatabaseRepository.SaveNotification(notifications);
          
            return notifications;
        }


      
    }
}
