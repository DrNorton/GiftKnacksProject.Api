using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackAgentCore.Services;
using GiftKnackProject.NotificationTypes.Chat;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnackMessageAgent.Services
{
    public class ChatMessageFromMqProcessor : IChatMessageFromMqProcessor
    {
        private readonly INoSqlDatabaseRepository _noSqlDatabaseRepository;

        public ChatMessageFromMqProcessor(INoSqlDatabaseRepository noSqlDatabaseRepository)
        {
            _noSqlDatabaseRepository = noSqlDatabaseRepository;
        }

        public async Task ProcessMessage(BrokeredMessage brokeredMessage)
        {
            var body = brokeredMessage.GetBody<string>();
            var message = JsonConvert.DeserializeObject<ChatMqMessage>(body);
            await _noSqlDatabaseRepository.SaveMessageToLastMessages(message);
            await _noSqlDatabaseRepository.SaveMessageToAllMessages(message);
            return;
        }
    }
}
